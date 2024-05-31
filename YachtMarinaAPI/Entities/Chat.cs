using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message>? Messages { get; set; } = new List<Message>();
        public List<User> Users { get; set; }
    }
}
