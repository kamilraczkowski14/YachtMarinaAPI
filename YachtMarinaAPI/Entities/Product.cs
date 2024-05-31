namespace YachtMarinaAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long LoanPricePerDay { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public decimal Length { get; set; }
        public int YearOfProduction { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }
        public string? PublicId { get; set; }
    }
}
