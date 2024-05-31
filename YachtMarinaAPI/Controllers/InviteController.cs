using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("invite")]
    [Authorize]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _service;

        public InviteController(IInviteService service)
        {
            _service = service;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery] CreateInviteDto dto)
        {
            var inviteId = await _service.CreateInvite(dto);

            return Created($"/{inviteId}", null);

        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Invite>>> GetAll()
        {
            var invites = await _service.GetAll();

            return Ok(invites);
        }

        [HttpDelete("{id}/deny")]
        public async Task<IActionResult> Deny(int id)
        {
            await _service.DenyInvite(id);
            return Ok();
        }

        [HttpDelete("{id}/accept")]
        public async Task<IActionResult> Accept(int id)
        {
            await _service.AcceptInvite(id);
            return Ok();
        }
    }

}
