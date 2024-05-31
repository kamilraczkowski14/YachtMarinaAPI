using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("friend")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _service;

        public FriendController(IFriendService service)
        {
            _service = service;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Friend>>> GetAll()
        {
            var friends = await _service.GetAll();

            return Ok(friends);
        }


        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }

    }
}
