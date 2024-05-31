namespace YachtMarinaAPI.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public bool isLoan { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }


    }
}
