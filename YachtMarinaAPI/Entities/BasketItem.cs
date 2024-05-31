namespace YachtMarinaAPI.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;
        public int ProductId { get; set; }
        public bool isLoan { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public long Price { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
