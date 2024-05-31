using Microsoft.AspNetCore.SignalR;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Hubs;
using static System.Net.Mime.MediaTypeNames;
using YachtMarinaAPI.Models;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Services
{
    public interface IChatHubService
    {
        public Task JoinRoom(string connectionId, string roomName);
        public Task SendMessage(CreateMessageDto dto, int chatId, string roomName);
    }

    public class ChatHubService : IChatHubService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chat;
        private readonly IUserContextService _userContextService;

        public ChatHubService(ApplicationDbContext context, IHubContext<ChatHub> chat, IUserContextService userContextService)
        {
            _context = context;
            _chat = chat;
            _userContextService = userContextService;
        }

        public async Task JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
        }

        public async Task SendMessage(CreateMessageDto dto, int chatId, string roomName)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(u => u.Id == _userContextService.LoggedUserId);

            var newMessage = new Message
            {
                ChatId = chatId,
                MessageText = dto.MessageText,
                Username = user.Username,
                Time = DateTime.Now,
                UserId = user.Id,
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            await _chat.Clients.Group(roomName).SendAsync("ReceiveMessage", newMessage);
        }
    }
}
