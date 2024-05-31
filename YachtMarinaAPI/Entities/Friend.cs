namespace YachtMarinaAPI.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? AvatarUrl { get; set; }
        public int FriendUserId { get; set; }
        public string Rolename { get; set; }
    }
}
