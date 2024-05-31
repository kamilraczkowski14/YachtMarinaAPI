 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccessController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccessService _service;
        private readonly IEmailService _emailService;

        public AccessController(ApplicationDbContext context, IAccessService service, IEmailService emailService)
        {
            _context = context;
            _service = service;
            _emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _service.Register(dto);

            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);

            var token = user.ConfirmationToken;

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Access", new {token},
                Request.Scheme);


            var message = new EmailMessage(new string[]
            {
                user.Email,
            }, "YachtMarina rejestracja konta", "Kliknij, aby potwierdzić rejestrację " + confirmationLink);

            _emailService.SendEmail(message);

            return Created("/login", null);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var userDto = await _service.Login(dto);

            return Ok(userDto);
        }

        [HttpGet("getLoggedUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> getLoggedUser()
        {
            var userDto = await _service.loggedUser();
            return Ok(userDto);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            await _service.ConfirmEmail(token);

            return Ok("Poprawnie zweryfikowano konto");
        }

    }
}
