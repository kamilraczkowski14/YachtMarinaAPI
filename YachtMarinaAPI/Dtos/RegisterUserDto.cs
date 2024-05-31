namespace YachtMarinaAPI.Dtos
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int? RoleId { get; set; } = 1;
        public DateTime DateOfRegister { get; set; } = DateTime.Now;
    }
}
