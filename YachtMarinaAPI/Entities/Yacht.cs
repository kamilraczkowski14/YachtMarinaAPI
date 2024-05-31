namespace YachtMarinaAPI.Models
{
    public class Yacht
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string PictureUrl { get; set; }
        public bool isLoan { get; set; }
        public int YearOfProduction { get; set; }
        public decimal Length { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int userId { get; set; }
        public User User { get; set; }

    }
}
