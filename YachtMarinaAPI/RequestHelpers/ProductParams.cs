namespace YachtMarinaAPI.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string OrderBy { get; set; } = "name";
        public string? SearchTerm { get; set; }
        public string? Types { get; set; }
        public string? Brands { get; set; }

    }
}
