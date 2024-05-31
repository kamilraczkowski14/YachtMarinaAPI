namespace YachtMarinaAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string Filename { get; set; }
        public string RoleName { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
