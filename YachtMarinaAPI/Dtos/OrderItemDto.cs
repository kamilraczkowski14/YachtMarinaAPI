namespace YachtMarinaAPI.Dtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; } = 1;
        public bool isLoan { get; set; }
    }
}
