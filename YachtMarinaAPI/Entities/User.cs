using YachtMarinaAPI.Entities;

namespace YachtMarinaAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime DateOfRegister { get; set; }
        public string? PublicId { get; set; }
        public int? RoleId { get; set; } = 1;
        public string? ConfirmationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public virtual Role Role { get; set; }
        public List<Yacht>? Yachts { get; set; }
        public List<Journey>? Journeys { get; set; }
        public List<Friend>? Friends { get; set; }
        public List<Invite>? Invites { get; set; }
        public List<Chat>? Chats { get; set; } 

    }
}
