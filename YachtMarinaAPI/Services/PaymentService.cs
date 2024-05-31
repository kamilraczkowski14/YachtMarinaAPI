using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.Models.Order;

namespace YachtMarinaAPI.Services
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdateRequest();
        ActionResult ConnectStripe();
    }
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, ApplicationDbContext context,
            IUserContextService userContextService, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _context = context;
            _userContextService = userContextService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BasketDto> CreateOrUpdateRequest()
        {
            var basket = _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(b => b.UserId == _userContextService.LoggedUserId);

            if (basket == null)
            {
                throw new NotFoundException("Nie masz utworzonego koszyka");
            }


            var intent = PaymentIntent(basket); 

            if (intent == null)
            {
                throw new BadRequestException("Coś poszło nie tak z płatnością");
            }

            basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
            basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

            _context.Update(basket);
            await _context.SaveChangesAsync();

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret,
                BasketItems = basket.BasketItems.Select(item => new BasketItemDto
                {
                    Id = item.Id,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    Price = item.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity,
                    isLoan = item.isLoan,
                    startDate = item.startDate,
                    endDate = item.endDate
                }).ToList()
            };

        }

        public ActionResult ConnectStripe()
        {
            var json = new StreamReader(_httpContextAccessor.HttpContext.Request.Body).ReadToEnd();

            var stripeEvent = EventUtility.ConstructEvent(json, _httpContextAccessor.HttpContext.Request.Headers["Stripe-Signature"],
                _configuration["StripeSettings:WhSecret"]);

            var charge = (Charge)stripeEvent.Data.Object;

            var order = _context.Orders
                .FirstOrDefault(x => x.PaymentIntentId == charge.PaymentIntentId);

            if (charge.Status == "succeeded")
            {
                order.OrderStatus = OrderStatus.PaymentReceived;
            }

            _context.SaveChanges();

            return new EmptyResult();
        }

        private PaymentIntent PaymentIntent(Basket basket)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            var intent = new PaymentIntent();

            var subtotal = basket.BasketItems.Sum(item => item.Quantity * item.Price);

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = subtotal * 100,
                    Currency = "pln",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = service.Create(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = subtotal * 100
                };

                service.Update(basket.PaymentIntentId, options);
            }

            return intent;
        }
    }
}
