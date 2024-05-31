using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfRegister { get; set; }
        public Role Role { get; set; }
        public List<YachtDto>? Yachts { get; set; }
        public List<JourneyDto>? Journeys { get; set; }
        public List<Friend>? Friends { get; set; }
        public List<Invite>? Invites { get; set; }
        public List<ChatDto>? Chats { get; set; }
    }
}
