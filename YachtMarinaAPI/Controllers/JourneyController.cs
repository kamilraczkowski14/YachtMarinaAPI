using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("journeys")]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyService _service;

        public JourneyController(IJourneyService service)
        {
            _service = service;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Start([FromBody] CreateJourneyDto dto)
        {
            var id = await _service.Start(dto);

            return Created($"{id}", null);
        }


        [HttpGet("getAll")]
        public async Task<ActionResult<List<JourneyDto>>> GetAll()
        {
            var journeys = await _service.GetAll();

            return Ok(journeys);

        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }

        [HttpPut("{id}/end")]
        public async Task<IActionResult> End([FromRoute]int id)
        {
            await _service.End(id);
            return Ok();
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var journey = await _service.GetById(id);

            return Ok(journey);
        }


        [HttpPut("{id}/addNote")]
        public async Task<IActionResult> AddNote([FromRoute] int id, [FromBody] CreateNoteDto dto)
        {
            await _service.AddNote(id, dto);

            return Ok();
        }

        [HttpPut("{id}/addPhoto")]
        public async Task<IActionResult> AddPhoto([FromRoute] int id, [FromForm] AddPhotoDto dto)
        {
            await _service.AddPhoto(id, dto);

            return Ok();
        }


        [HttpGet("{id}/getYacht")]
        public async Task<IActionResult> GetYacht([FromRoute] int id)
        {
            var yacht = await _service.GetYacht(id);

            return Ok(yacht);
        }

        [HttpGet("{id}/getUsers")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            var friends = await _service.GetUsers(id);

            return Ok(friends);
        }
    }
}
