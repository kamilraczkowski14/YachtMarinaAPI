namespace YachtMarinaAPI.Dtos
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? File { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
