using YachtMarinaAPI.Models.Order;

namespace YachtMarinaAPI.Dtos
{
    public class CreateOrderDto
    {
        public ShippingAddress ShippingAddress { get; set; }
    }
}
