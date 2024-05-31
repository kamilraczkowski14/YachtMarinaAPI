using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("basket")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;

        public BasketController(IBasketService service)
        {
            _service = service;
        }

        [HttpGet("getBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await _service.GetBasket();

            return Ok(basket);
        }


        [HttpPost("addLoanItem")]
        public async Task<ActionResult<BasketDto>> AddItemToBasketLoan(SetItemDateDto dto,
            bool isloan, int productId, int quantity)
        {
            var basket = await _service.AddItemToBasketLoan(dto, isloan, productId, quantity);

            return Created("basket/getBasket", basket);

        }

        [HttpPost("addItem")]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(bool isloan, int productId, int quantity)
        {
            var basket = await _service.AddItemToBasket(isloan, productId, quantity);

            return Created("basket/getBasket", basket);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveItemFromBasket(int Id, int quantity)
        {
            await _service.RemoveItemFromBasket(Id, quantity);
            return Ok();
        }
    }
}
