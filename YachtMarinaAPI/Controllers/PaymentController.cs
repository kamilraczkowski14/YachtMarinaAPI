using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }



        [HttpPost("create")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
        {
            var basket = await _service.CreateOrUpdateRequest();

            return basket;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var emptyResult = _service.ConnectStripe();

            return emptyResult;
        }
    }
}
