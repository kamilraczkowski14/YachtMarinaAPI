
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("yachts")]
    [ApiController]
    [Authorize]
    public class YachtController : ControllerBase
    {
        private readonly IYachtService _service;

        public YachtController(IYachtService service)
        {
            _service = service;
        }

        [HttpGet("getYachts")]
        public async Task<IActionResult> GetAll()
        {
            var yachts = await _service.GetAll();

            return Ok(yachts);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateYachtDto dto)
        {
            var id = await _service.Create(dto);

            return Created($"yachts/{id}", null);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromForm] UpdateYachtDto dto)
        {
            await _service.Edit(dto);

            return Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.Delete(id);
            return Ok();
        }

    }
}
