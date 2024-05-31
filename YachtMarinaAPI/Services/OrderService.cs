using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.Models.Order;

namespace YachtMarinaAPI.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderById(int orderId);
        Task<int> CreateOrder(CreateOrderDto dto);
    }
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public OrderService(ApplicationDbContext context, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = _context.Orders
                .Where(o => o.UserId == _userContextService.LoggedUserId)
                .ToList();

            return orders;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var order = _context.Orders
                .Where(o => o.UserId == _userContextService.LoggedUserId && o.Id == orderId)
                .FirstOrDefault();

            if (order == null)
            {
                throw new NotFoundException("Nie znaleziono zamowienia");
            }

            return order;
        }


        public async Task<int> CreateOrder(CreateOrderDto dto)
        {

            var basket = _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(b => b.UserId == _userContextService.LoggedUserId);

            if (basket == null)
            {
                throw new BadRequestException("Nie dodales rzeczy do koszyka");
            }

            var items = new List<OrderItem>();
            var yachts = new List<Yacht>();

            foreach (var item in basket.BasketItems)
            {

                var productItem = _context.Products.Find(item.ProductId);

                if (productItem == null)
                {
                    throw new NotFoundException("Nie znaleziono produktu");
                }

                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    isLoan = item.isLoan,
                    startDate = item.startDate,
                    endDate = item.endDate
                };
                

                var yacht = new Yacht
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    Type = productItem.Type,
                    Brand = productItem.Brand,
                    PictureUrl = productItem.PictureUrl,
                    isLoan = item.isLoan,
                    YearOfProduction = productItem.YearOfProduction,
                    Length = productItem.Length,
                    startDate = item.startDate,
                    endDate = item.endDate,
                    userId = (int)_userContextService.LoggedUserId
                };

                yachts.Add(yacht);

                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            foreach (var yacht in yachts)
            {
                _context.Yachts.Add(yacht);
            }


            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                //OrderItems = items,
                UserId = (int)_userContextService.LoggedUserId,
                ShippingAddress = dto.ShippingAddress,
                Subtotal = subtotal,
                PaymentIntentId = basket.PaymentIntentId
            };


            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();

            var orderId = order.Id;

            return orderId;

        }

    }

}
