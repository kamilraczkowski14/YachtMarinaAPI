using Microsoft.AspNetCore.SignalR;

namespace YachtMarinaAPI.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}
