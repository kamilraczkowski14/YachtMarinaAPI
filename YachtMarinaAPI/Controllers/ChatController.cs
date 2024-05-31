using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service)
        {
            _service = service;
        }

        [HttpGet("{chatId}/getMessages")]
        public async Task<IActionResult> GetAllMessages([FromRoute]int chatId)
        {
            var messages = await _service.GetAllMessages(chatId);
            return Ok(messages);
        }

        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetById(int id)
        {
            var chat = await _service.GetById(id);

            return Ok(chat);
        }

        
        [HttpPost("create/{userId}")]
        public async Task<IActionResult> CreatePrivateChat([FromRoute]int userId)
        {
            var id = await _service.CreatePrivateChat(userId);

            return Created($"/{id}", null);
        }


        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPrivates()
        {
            var chats = await _service.GetAllPrivates();

            return Ok(chats);
        }

        [HttpGet("getChatsWithAdmins")]
        public async Task<IActionResult> GetChatsWithAdmins()
        {
            var chats = await _service.GetChatsWithAdmins();

            return Ok(chats);
        }

        [HttpGet("getChatsForAdmins")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> GetChatsForAdmins()
        {
            var chats = await _service.GetChatsForAdmins();

            return Ok(chats);
        }
    }
}
