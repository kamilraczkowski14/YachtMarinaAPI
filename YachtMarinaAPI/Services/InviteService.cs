using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;

namespace YachtMarinaAPI.Services
{
    public interface IInviteService
    {
        Task<int> CreateInvite(CreateInviteDto dto);
        Task AcceptInvite(int inviteId);
        Task DenyInvite(int inviteId);
        Task<List<Invite>> GetAll();

    }
    public class InviteService : IInviteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public InviteService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<int> CreateInvite(CreateInviteDto dto)
        {
            var newInvite = _mapper.Map<Invite>(dto);

            var user = _context.Users
                .Include(u => u.Friends)
                .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            if (user == null)
            {
                throw new BadRequestException("Coś poszło nie tak");
            }

            newInvite.FromUserId = user.Id;
            newInvite.FromUsername = user.Username;
            newInvite.FromUserAvatarUrl = user.AvatarUrl;


            if (newInvite.FromUserId == newInvite.ToUserId)
            {
                throw new BadRequestException("Nie można wysłać zaproszenia do samego siebie!");
            }

            var existingFriend = user.Friends.FirstOrDefault(f => f.FriendUserId == newInvite.ToUserId);

            if (existingFriend != null)
            {
                throw new BadRequestException("Nie mozna wyslac zaproszenia do znajomego!");
            }

            var invitedUser = _context.Users
                .Include(u => u.Invites)
                .FirstOrDefault(u => u.Id == newInvite.ToUserId);

            if (invitedUser == null)
            {
                throw new BadRequestException("Nie ma takiego użytkownika");
            }


            var existingInvite = _context.Invites
    .FirstOrDefault(i => i.FromUserId == newInvite.FromUserId && i.ToUserId == newInvite.ToUserId);

            if (existingInvite != null)
            {
                throw new BadRequestException("Wysłałeś już zaproszenie do tego użytkownika");
            }

            invitedUser.Invites.Add(newInvite);

            _context.Invites.Add(newInvite);
            await _context.SaveChangesAsync();

            return newInvite.Id;
        }

        public async Task AcceptInvite(int inviteId)
        {
            var invite = _context.Invites.FirstOrDefault(i => i.Id == inviteId);

            if (invite.ToUserId != _userContextService.LoggedUserId)
            {
                throw new ForbidException("Nie masz uprawnień dla tego działania");
            }

            if (invite == null)
            {
                throw new NotFoundException("Nie znaleziono zaproszenia");
            }

            var userTo = _context.Users
                .Include(u => u.Friends)
                .Include(u => u.Invites)
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == invite.ToUserId);

            if (userTo == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            }

            var userFrom = _context.Users
                .Include(u => u.Friends)
                .Include(u => u.Invites)
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == invite.FromUserId);

            if (userFrom == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            }

            var userFromDto = new Friend
            {
                Username = userFrom.Username,
                AvatarUrl = userFrom.AvatarUrl,
                FriendUserId = userFrom.Id,
                Rolename = userFrom.Role.Rolename
            };

            var userToDto = new Friend
            {
                Username = userTo.Username,
                AvatarUrl = userTo.AvatarUrl,
                FriendUserId = userTo.Id,
                Rolename = userTo.Role.Rolename
            };

            userTo.Friends.Add(userFromDto);
            userFrom.Friends.Add(userToDto);

            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
        }

        public async Task DenyInvite(int inviteId)
        {
            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == inviteId);

            if (invite.ToUserId != _userContextService.LoggedUserId)
            {
                throw new ForbidException("Nie masz uprawnień dla tego działania");
            }

            if (invite == null)
            {
                throw new NotFoundException("Nie znaleziono zaproszenia");
            }

            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Invite>> GetAll()
        {
            var invites = _context.Invites.Where(i => i.ToUserId == _userContextService.LoggedUserId).ToList();

            return invites;
        }
    }
}
