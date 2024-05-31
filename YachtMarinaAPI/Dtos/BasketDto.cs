namespace YachtMarinaAPI.Dtos
{
    public class BasketDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
    }
}
