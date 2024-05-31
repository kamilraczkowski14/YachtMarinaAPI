using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Models.Order;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Authorize]
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }



        [HttpGet("getAll")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _service.GetOrders();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetById([FromRoute] int orderId)
        {
            var order = await _service.GetOrderById(orderId);
            return Ok(order);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Order>> Create(CreateOrderDto dto)
        {
            var orderId = await _service.CreateOrder(dto);

            return Created($"orders/{orderId}", null);
        }
    }
}
