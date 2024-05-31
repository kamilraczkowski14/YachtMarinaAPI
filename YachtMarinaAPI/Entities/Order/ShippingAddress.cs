using Microsoft.EntityFrameworkCore;

namespace YachtMarinaAPI.Models.Order
{
    [Owned]
    public class ShippingAddress
    {
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}
