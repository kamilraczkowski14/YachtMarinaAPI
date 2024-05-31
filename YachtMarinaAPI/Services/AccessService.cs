using AutoMapper;
using Azure.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using RestSharp;
using RestSharp.Authenticators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IAccessService
    {
        Task Register(RegisterUserDto dto);
        Task<UserDto> Login(LoginUserDto dto);
        Task<UserDto> loggedUser();
        Task ConfirmEmail(string token);
    }
    public class AccessService : IAccessService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public AccessService(ApplicationDbContext context, AuthenticationSettings authenticationSettings,
            IPasswordHasher<User> passwordHasher, IUserContextService userContextService, IMapper mapper, 
            IEmailService emailService)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<UserDto> loggedUser()
        {
            var loggedUserId = _userContextService.LoggedUserId;


            var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Yachts)
            .Include(u => u.Journeys)
            .Include(u => u.Friends)
            .Include(u => u.Invites)
            .Include(u => u.Chats)
            .FirstOrDefaultAsync(u => u.Id == loggedUserId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono użytkownika");
            }



            if (user.Yachts != null)
            {
                foreach (var yacht in user.Yachts)
                {
                    if (yacht.endDate != null)
                    {
                        DateTime currentDate = DateTime.Now.Date;
                        DateTime endDate = yacht.endDate.Value.Date;

                        if (endDate < currentDate)
                        {
                            var productItem = await _context.Products.FindAsync(yacht.ProductId);

                            _context.Yachts.Remove(yacht);

                            productItem.QuantityInStock += 1;

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }


            var yachtsDtos = _mapper.Map<List<YachtDto>>(user.Yachts);
            var journeysDtos = _mapper.Map<List<JourneyDto>>(user.Journeys);
            //var chatsDtos = _mapper.Map<List<ChatDto>>(user.Chats);

            var chats = await _context.Chats
             .Include(c => c.Users)
             .Include(c => c.Messages)
             .Where(c => c.Users.Any(u => u.Id == _userContextService.LoggedUserId))
             .ToListAsync();

            var filteredChats = new List<Chat>();

            foreach (var chat in chats)
            {
                var user1 = chat.Users
                    .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

                var loggedUser = _context.Users
                    .Include(u => u.Friends)
                    .FirstOrDefault(u => u.Id == user1.Id);

                if (loggedUser != null)
                {
                    var otherUser = chat.Users.FirstOrDefault(u => u.Id != _userContextService.LoggedUserId);

                    if (otherUser == null)
                    {
                        throw new NotFoundException("Nie znaleziono uzytkownika");
                    }

                    var friend = loggedUser.Friends.FirstOrDefault(f => f.FriendUserId == otherUser.Id);

                    if (friend != null)
                    {
                        filteredChats.Add(chat);
                    }

                }
            }

            var chatsDtos = _mapper.Map<List<ChatDto>>(filteredChats);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = GenerateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Role = user.Role,
                DateOfRegister = user.DateOfRegister,
                AvatarUrl = user.AvatarUrl,
                Yachts = yachtsDtos,
                Journeys = journeysDtos,
                Friends = user.Friends,
                Invites = user.Invites,
                Chats = chatsDtos
            };
        }
        public async Task Register(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                RoleId = dto.RoleId,
                DateOfRegister = dto.DateOfRegister,
                ConfirmationToken = GenerateConfirmationToken()
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.HashPassword = hashedPassword;

            var existingUsername = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            var existingEmail = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (existingUsername != null)
            {
                throw new BadRequestException("Ta nazwa użytkownika jest już zajęta");
            }

            if (existingEmail != null)
            {
                throw new BadRequestException("Ten adres e-mail jest już zajęty");
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();


            var admins = await _context.Users
            .Where(u => u.RoleId == 8 || u.RoleId == 7)
            .ToListAsync();

            if( (newUser.RoleId) != 8 && (newUser.RoleId != 7) )
            {
                if (admins != null)
                {
                    foreach (var admin in admins)
                    {
                        var newChat = new Chat
                        {
                            Name = "Czat " + newUser.Username + " - " + admin.Username,
                            Users = new List<User>()
                        };

                        newChat.Users.Add(newUser);
                        newChat.Users.Add(admin);

                        newUser.Chats ??= new List<Chat>();
                        newUser.Chats.Add(newChat);

                        admin.Chats ??= new List<Chat>();
                        admin.Chats.Add(newChat);

                        _context.Chats.Add(newChat);
                    }
                }
            }
            

            if ( (newUser.RoleId == 8) || (newUser.RoleId == 7) )
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Chats)
                    .ToListAsync();

                foreach (var user in users)
                {
                    if( (user.RoleId != 8) && (user.RoleId != 7)) 
                    {
                        if (user.Id != newUser.Id)
                        {
                            var newChat = new Chat
                            {
                                Name = "Czat " + user.Username + " - " + newUser.Username,
                                Users = new List<User>()
                            };

                            newChat.Users.Add(newUser);
                            newChat.Users.Add(user);

                            newUser.Chats ??= new List<Chat>();
                            newUser.Chats.Add(newChat);

                            user.Chats ??= new List<Chat>();
                            user.Chats.Add(newChat);

                            /*
                            if (user.Chats == null)
                            {
                                user.Chats = new List<Chat>();
                            }

                            user.Chats.Add(newChat);
                            newUser.Chats.Add(newChat);
                            */

                            _context.Chats.Add(newChat);
                        }
                    }
                    
                }
            }



            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> Login(LoginUserDto dto)
        {
            var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Yachts)
            .Include(u => u.Journeys)
            .Include(u => u.Friends)
            .Include(u => u.Invites)
            .Include(u => u.Chats)
            .FirstOrDefaultAsync(u => u.Username == dto.Name);

            if (user == null)
            {
                user = await _context.Users
                       .Include(u => u.Role)
                        .Include(u => u.Yachts)
                        .Include(u => u.Journeys)
                        .Include(u => u.Friends)
                        .Include(u => u.Invites)
                        .Include(u => u.Chats)
                    .FirstOrDefaultAsync(u => u.Email == dto.Name);
            }

            if (user == null)
            {
                throw new BadRequestException("Niepoprawna nazwa użytkownika lub hasło");
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Niepoprawna nazwa użytkownika lub hasło");
            }

            
            if(user.VerifiedAt == null)
            {
                throw new BadRequestException("Aby zalogować się potwierdź rejestrację za" +
                    " pomocą linka wysłanego na adres e-mail");
            }
            

            
            var yachtsDtos = _mapper.Map<List<YachtDto>>(user.Yachts);
            var journeysDtos = _mapper.Map<List<JourneyDto>>(user.Journeys);
            var chatsDtos = _mapper.Map<List<ChatDto>>(user.Chats);


            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = GenerateToken(user),
                AvatarUrl = user.AvatarUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                DateOfRegister = user.DateOfRegister,
                Yachts = yachtsDtos,
                Journeys = journeysDtos,
                Friends = user.Friends,
                Invites = user.Invites,
                Chats = chatsDtos,
                Role = user.Role,
            };

        }

        public async Task ConfirmEmail(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            if(user == null)
            {
                throw new BadRequestException("Weryfikacja konta nie powiodła się");
            }

            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }



        private string GenerateToken(User user)
        {
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, $"{user.Role.Rolename}")
            };

            if (user.DateOfBirth.HasValue)
            {
                authClaims.Add(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyy-MM-dd")));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                authClaims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


        private string GenerateConfirmationToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

    }
}
