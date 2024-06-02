using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Collections.Generic;
using System.Linq;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Hubs;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IChatService
    {
        public Task<ChatDto> GetById(int id);
        public Task<int> CreatePrivateChat(int userId);
        public Task<List<ChatDto>> GetAllPrivates();
        public Task<List<ChatDto>> GetChatsWithAdmins();
        public Task<List<ChatDto>> GetChatsForAdmins();
        public Task<List<MessageDto>> GetAllMessages(int chatId);
        
    }
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public ChatService(ApplicationDbContext context, IUserContextService userContextService, IMapper mapper)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<int> CreatePrivateChat(int userId)
        {
            var user =  _context.Users.FirstOrDefault(u => u.Id==userId);

            var loggedUser =  _context.Users
                .Include(u => u.Friends)
                .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);
 
            if(loggedUser == null || user == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            } 
            
            if(_userContextService.LoggedUserId == userId)
            {
                throw new BadRequestException("Nie możesz utworzyć czatu z samym sobą");
            }

            
            var friend = loggedUser.Friends.FirstOrDefault(f => f.FriendUserId == userId);

            
            if(friend == null)
            {
                throw new BadRequestException("Nie możesz wysłać wiadomości do tego użytkownika. Nie jest twoim znajomym");
            }
          
            
            var existingChat = _context.Chats
                .Include(c => c.Users)
                .FirstOrDefault(c => c.Users.Any(u => u.Id == loggedUser.Id) &&
                         c.Users.Any(u => u.Id == user.Id)); 

            if(existingChat != null)
            {
                return existingChat.Id;
            }
            

            var newChat = new Chat
            {
                Name = "Czat " + loggedUser.Username + " - " + user.Username,
                Users = new List<User>()
            };

            newChat.Users.Add(loggedUser);
            newChat.Users.Add(user);

            if (user.Chats == null)
            {
                user.Chats = new List<Chat>(); 
            }

            user.Chats.Add(newChat);

            if (loggedUser.Chats == null)
            {
                loggedUser.Chats = new List<Chat>(); 
            }

            loggedUser.Chats.Add(newChat);


            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();

            return newChat.Id;
        }

        public async Task<List<MessageDto>> GetAllMessages(int chatId)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            var user = _context.Users.FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            if (chat == null)
            {
                throw new NotFoundException("Nie znaleziono czatu");
            }

            if (!chat.Users.Any(u => u.Id == _userContextService.LoggedUserId))
            {
                throw new ForbidException("Brak dostępu do czatu");
            }

            var messages = chat.Messages.ToList();

            var unseenMessages = chat.Messages.Where(m => m.isSeen == false).ToList();

            foreach (var message in unseenMessages)
            {
                if (message.Username != user.Username)
                {
                    message.isSeen = true;
                }
                await _context.SaveChangesAsync();
            }

            var messagesDto = _mapper.Map<List<MessageDto>>(messages);

            

            return messagesDto;
        }

        public async Task<List<ChatDto>> GetAllPrivates()
        {
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

                    if(otherUser == null)
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

            return chatsDtos;
        }

        public async Task<List<ChatDto>> GetChatsWithAdmins()
        {
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

                    if (friend == null)
                    {
                        filteredChats.Add(chat);
                    }

                }
            }

            var chatsDtos = _mapper.Map<List<ChatDto>>(filteredChats);

            return chatsDtos;
        }

        
        public async Task<List<ChatDto>> GetChatsForAdmins()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.LoggedUserId);

            if(user == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            }

            if(user.RoleId != 7 && user.RoleId != 8)
            {
                throw new ForbidException("Nie masz uprawnień dla tej operacji");
            }

            var chats = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .Where(c => c.Users.Any(u => u.Id == user.Id))
                .ToListAsync();

            var filteredChats = new List<Chat>(); 

            foreach (var chat in chats)
            {
                if (chat.Messages.Count > 0)
                {
                    filteredChats.Add(chat); 
                }
            }

            var chatsDtos = _mapper.Map<List<ChatDto>>(filteredChats);

            return chatsDtos;
        }
        

        public async Task<ChatDto> GetById(int id)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            var user = _context.Users
                .Include(u => u.Chats)
                .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            //if (user == null)
            //{
                //throw new NotFoundException("Nie znaleziono użytkownika");
            //}

            if (chat == null)
            {
                throw new NotFoundException("Nie znaleziono czatu");
            }

            if (!chat.Users.Any(u => u.Id == _userContextService.LoggedUserId))
            {
                throw new ForbidException("Brak dostępu do czatu");
            }

            var chatDto = _mapper.Map<ChatDto>(chat);

            

            return chatDto;

        }

    }
}
