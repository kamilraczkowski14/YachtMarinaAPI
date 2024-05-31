namespace YachtMarinaAPI.Dtos
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Filename { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
