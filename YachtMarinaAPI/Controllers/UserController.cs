using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.RequestHelpers;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPut("completeProfile")]
        public async Task<IActionResult> CompleteProfile([FromBody] UpdateUserDto dto)
        {
            await _service.CompleteProfile(dto);

            return Ok("Uzupelniles informacje o sobie");

        }

        [HttpPut("changeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm] UpdateUserDto dto)
        {
            await _service.ChangeAvatar(dto);
            return Ok("Zmieniles swoje zdjecie!");
        }

        [HttpPut("changeUsername")]
        public async Task<IActionResult> ChangeUsername([FromBody] UpdateUserDto dto)
        {
            await _service.ChangeUsername(dto);

            return Ok("Zmieniles nazwe uzykownika!");

        }

        [HttpPut("changeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] UpdateUserDto dto)
        {
            await _service.ChangeEmail(dto);

            return Ok("Zmieniles email!");

        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserDto dto)
        {
            await _service.ChangePassword(dto);

            return Ok("Zmieniles haslo!");

        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int userId)
        {
            var user = await _service.GetUserById(userId);

            return Ok(user);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            await _service.DeleteUser();

            return Ok("Usunales konto");
        }


        [HttpGet("getAvatarUrl")]
        public async Task<IActionResult> GetAvatarUrl()
        {
            var avatarDto = await _service.GetUserAvatarUrl();

            return Ok(avatarDto);
        }


        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<User>>> GetAll([FromQuery] UserParams userParams)
        {
            var users = await _service.GetAll(userParams);

            return Ok(users);
        }

    }
}
