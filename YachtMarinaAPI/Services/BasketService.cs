using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IBasketService
    {
        Task<BasketDto> AddItemToBasketLoan(SetItemDateDto dto, bool isloan, int productId, int quantity = 1);
        Task<BasketDto> AddItemToBasket(bool isloan, int productId, int quantity = 1);
        Task RemoveItemFromBasket(int productId, int quantity);
        Task<BasketDto> GetBasket();
    }
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;

        public BasketService(ApplicationDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<BasketDto> AddItemToBasketLoan(SetItemDateDto dto, bool isloan, int productId, int quantity = 1)
        {

            var basket = await basketget(GetUserId());

            if (basket == null)
            {
                basket = CreateBasket();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException("Nie znaleziono takiego produktu");
            }

            //if (basket.BasketItems.All(item => item.ProductId != product.Id))
            //{

            DateTime currentDate = DateTime.Now.Date;

            DateTime startDate = dto.startDate.Value.Date;
            DateTime endDate = dto.endDate.Value.Date;
            TimeSpan duration = endDate.Subtract(startDate);
            int numberOfDays = (int)duration.TotalDays + 1;


            if (dto.startDate < currentDate || dto.endDate < currentDate)
            {
                throw new BadRequestException("Nie można wybrać dat wcześniejszych niż aktualna data.");
            }

            if (dto.startDate > dto.endDate)
            {
                throw new BadRequestException("Wybrałeś złe daty");
            }

            basket.BasketItems.Add(new BasketItem
            {
                Product = product,
                ProductId = productId,
                Quantity = quantity,
                Price = product.LoanPricePerDay * numberOfDays,
                isLoan = isloan,
                startDate = dto.startDate.Value.Date,
                endDate = dto.endDate.Value.Date
            });

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
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Price,
                    Description = item.Product.Description,
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

        public async Task<BasketDto> AddItemToBasket(bool isloan, int productId, int quantity = 1)
        {

            var basket = await basketget(GetUserId());

            if (basket == null)
            {
                basket = CreateBasket();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException("Nie znaleziono takiego produktu");
            }

            //if (basket.BasketItems.All(item => item.ProductId != product.Id))
            //{

            basket.BasketItems.Add(new BasketItem
            {
                Product = product,
                ProductId = productId,
                Quantity = quantity,
                Price = product.Price,
                isLoan = isloan,
            });

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
                    Price = item.Price,
                    Description = item.Product.Description,
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

        public async Task<BasketDto> GetBasket()
        {

            var loggedUserId = GetUserId();

            if (loggedUserId == 0)
            {
                return null;
            }

            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(b => b.UserId == loggedUserId);


            if (basket == null)
            {
                return null;
            }


            /*
            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, basket,
                 new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Dzialanie zabronione!");
            }
            */

            //var basketDto = _mapper.Map<BasketDto>(basket);


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
                    ProductId = item.ProductId,
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


            //return basketDto;
        }

        public async Task RemoveItemFromBasket(int Id, int quantity = 1)
        {
            var basket = await basketget(GetUserId());

            if (basket == null)
            {
                throw new NotFoundException("Nie znaleziono koszyka");
            }

            var item = basket.BasketItems.FirstOrDefault(item => item.Id == Id);

            if (item == null) return;

            item.Quantity -= quantity;

            if (item.Quantity == 0)
            {
                basket.BasketItems.Remove(item);
            }

            /*
            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, basket,
                 new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Dzialanie zabronione!");
            }
            */

            await _context.SaveChangesAsync();
        }



        private Basket CreateBasket()
        {
            var userId = _userContextService.LoggedUserId;

            var basket = new Basket
            {
                UserId = (int)userId
            };

            _context.Baskets.Add(basket);
            _context.SaveChanges();

            return basket;
        }

        private async Task<Basket> basketget(int userId)
        {

            var loggedUserId = GetUserId();

            if (loggedUserId == 0)
            {
                return null;
            }

            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
            {
                return null;
            }

            return basket;
        }

        private int GetUserId()
        {
            var loggedUserId = _userContextService.LoggedUserId;

            if (loggedUserId == 0)
            {
                return 0;
            }

            return (int)loggedUserId;

        }
    }
}
