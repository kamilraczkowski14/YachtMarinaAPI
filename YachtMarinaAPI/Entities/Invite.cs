namespace YachtMarinaAPI.Entities
{
    public class Invite
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public string FromUsername { get; set; }
        public string? FromUserAvatarUrl { get; set; }
        public int ToUserId { get; set; }
    }
}
