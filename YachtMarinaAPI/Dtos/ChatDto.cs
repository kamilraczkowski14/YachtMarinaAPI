using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Dtos
{
    public class ChatDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MessageDto> Messages { get; set; }
        public List<UserChatDto> Users { get; set; }
    }
}
