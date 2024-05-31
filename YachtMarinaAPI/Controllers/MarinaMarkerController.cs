using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Route("marinamarkers")]
    public class MarinaMarkerController : ControllerBase
    {
        private readonly IMarinaMarkerService _service;

        public MarinaMarkerController(IMarinaMarkerService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        [Authorize(Roles="Właściciel,Bosman")]
        public async Task<IActionResult> Add([FromBody] CreateMarkersDto dto)
        {
            await _service.Add(dto);
            return Ok();
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var markers = await _service.GetAll();
            return Ok(markers);
        }

    }
}
