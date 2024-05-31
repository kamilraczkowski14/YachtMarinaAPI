 namespace YachtMarinaAPI.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
