namespace YachtMarinaAPI.Dtos
{
    public class CreateYachtDto
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public string Type { get; set; }
        public decimal Length { get; set; }
        public int YearOfProduction { get; set; }
        public string Brand { get; set; }
        public bool? isLoan { get; set; } = false;
    }
}
