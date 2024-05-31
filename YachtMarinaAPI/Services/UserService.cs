using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.Authorization;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Extensions;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.RequestHelpers;

namespace YachtMarinaAPI.Services
{
    public interface IUserService
    {
        Task DeleteUser();
        Task<List<User>> GetAll(UserParams userParams);
        Task<UserDto> GetUserById(int userId);
        Task ChangePassword(UpdateUserDto dto);
        Task ChangeEmail(UpdateUserDto dto);
        Task ChangeUsername(UpdateUserDto dto);
        Task CompleteProfile(UpdateUserDto dto);
        Task ChangeAvatar(UpdateUserDto dto);
        Task<AvatarDto> GetUserAvatarUrl();

    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ImageService _imageService;

        public UserService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService, IPasswordHasher<User> passwordHasher,
            ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
            _passwordHasher = passwordHasher;
            _imageService = imageService;
        }

        public async Task ChangeAvatar(UpdateUserDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if (dto.File != null)
            {
                var imageResult = _imageService.AddImage(dto.File);

                user.AvatarUrl = imageResult.Result.SecureUrl.ToString();

                user.PublicId = imageResult.Result.PublicId;
            }

            await _context.SaveChangesAsync();
        }
        public async Task ChangeEmail(UpdateUserDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            user.Email = dto.Email;
            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(UpdateUserDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if (string.IsNullOrEmpty(dto.OldPassword))
            {
                throw new BadRequestException("Podaj swoje stare hasło");
            }

            if (!_passwordHasher.VerifyHashedPassword(user, user.HashPassword, dto.OldPassword)
                    .Equals(PasswordVerificationResult.Success))
            {
                throw new BadRequestException("Podałes niepawidłowe stare hasło");
            }

            var hashedPassword = _passwordHasher.HashPassword(user, dto.NewPassword);

            user.HashPassword = hashedPassword;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeUsername(UpdateUserDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            user.Username = dto.Username;
            await _context.SaveChangesAsync();
        }

        public async Task CompleteProfile(UpdateUserDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if (!string.IsNullOrEmpty(dto.FirstName))
            {
                user.FirstName = dto.FirstName;
            }

            if (string.IsNullOrEmpty(dto.FirstName))
            {
                user.FirstName = null;
            }

            if (!string.IsNullOrEmpty(dto.LastName))
            {
                user.LastName = dto.LastName;
            }

            if (string.IsNullOrEmpty(dto.LastName))
            {
                user.LastName = null;
            }

            if (dto.DateOfBirth.HasValue) // Sprawdzamy, czy DateOfBirth nie jest null
            {
                user.DateOfBirth = dto.DateOfBirth.Value.Date;
            }

            if (string.IsNullOrEmpty((dto.DateOfBirth).ToString()))
            {
                user.DateOfBirth = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser()
        {
            var user = GetUser((int)_userContextService.LoggedUserId);


            var usersWithLoggedUserAsFriend = _context.Users
            .Include(u => u.Friends)
            .Where(u => u.Friends.Any(f => f.FriendUserId == user.Id))
            .ToList();

            foreach (var u in usersWithLoggedUserAsFriend)
            {
                var friend = u.Friends.FirstOrDefault(f => f.FriendUserId == user.Id);
                u.Friends.Remove(friend); 
            }

            var chatsWithUser = _context.Chats
                 .Include(u => u.Users)
                 .Where(u => u.Users.Any(u => u.Id == user.Id));

            foreach(var chat in chatsWithUser)
            {

                _context.Chats.Remove(chat);
            }


            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll( UserParams userParams)
        {
            var query = _context.Users
             .Include(u => u.Role)
             .Include(u => u.Yachts)
             .Include(u => u.Journeys)
             .Include(u => u.Friends)
             .Include(u => u.Invites)
             .Include(u => u.Chats)
             .Search(userParams.SearchTerm).AsQueryable();


            if (string.IsNullOrEmpty(userParams.SearchTerm))
            {
                return new List<User>(); 
            }

            return await query.ToListAsync();
        }

        public async Task<AvatarDto> GetUserAvatarUrl()
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            var userDto = _mapper.Map<UserDto>(user);

            var avatarDto = _mapper.Map<AvatarDto>(userDto);

            return avatarDto;
        }

        public async Task<UserDto> GetUserById(int userId)
        {
            var user = _context.Users
             .Include(u => u.Role)
             .Include(u => u.Yachts)
             .Include(u => u.Journeys)
             .Include(u => u.Friends)
             .Include(u => u.Invites)
             .Include(u => u.Chats)
             .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono takiego uzytkownika");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;

        }



        private User GetUser(int userId)
        {
            var user = _context.Users
              .Include(u => u.Role)
              .Include(u => u.Yachts)
              .Include(u => u.Journeys)
              .Include(u => u.Friends)
              .Include(u => u.Invites)
              .Include(u => u.Chats)
              .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono takiego uzytkownika");
            }

            var authorizationResult = _authorization.AuthorizeAsync(
                _userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Nie masz uprawnien dla tej operacji");
            }

            return user;
        }
    }
}
