namespace YachtMarinaAPI.Dtos
{
    public class CreateDocumentDto
    {
        public int RoleId { get; set; }
        public IFormFile File { get; set; }
    }
}
