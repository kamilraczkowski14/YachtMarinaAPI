using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;

namespace YachtMarinaAPI.Services
{
    public interface IFriendService
    {
        Task<List<Friend>> GetAll();
        Task Delete(int friendId);
    }
    public class FriendService : IFriendService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public FriendService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task Delete(int friendId)
        {
            var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == _userContextService.LoggedUserId);

            var friendOne = user.Friends.FirstOrDefault(f => f.Id == friendId);

            if (friendOne == null)
            {
                throw new NotFoundException("Nie znaleziono takiego znajomego");
            }

            var userTwo = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == friendOne.FriendUserId);

            var friendTwo = userTwo.Friends.FirstOrDefault(f => f.FriendUserId == _userContextService.LoggedUserId);


            user.Friends.Remove(friendOne);
            userTwo.Friends.Remove(friendTwo);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Friend>> GetAll()
        {
            var user = _context.Users
                .Include(u => u.Friends)
                .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            }

            var userDto = _mapper.Map<UserDto>(user);

            var friends = userDto.Friends.ToList();

            return friends;
        }
    }
}
