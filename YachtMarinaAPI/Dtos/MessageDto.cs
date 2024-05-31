namespace YachtMarinaAPI.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string MessageText { get; set; }
        public int ChatId { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public bool isSeen { get; set; } = false;
    }
}
