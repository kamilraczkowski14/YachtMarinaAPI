namespace YachtMarinaAPI.Models.Order
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public long Subtotal { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }

    }
}
