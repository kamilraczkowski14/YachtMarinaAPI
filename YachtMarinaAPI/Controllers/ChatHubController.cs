using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("chathub")]
    public class ChatHubController : ControllerBase
    {
        private readonly IChatHubService _chat;

        public ChatHubController(IChatHubService chat)
        {
            _chat = chat;
        }
        

        [HttpPost("joinRoom/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.JoinRoom(connectionId, roomName);
            return Ok();
        }
        

        [HttpPost("sendMessage/{chatId}/{roomName}")]
        public async Task<IActionResult> SendMessage([FromForm] CreateMessageDto dto, [FromRoute]int chatId, [FromRoute]string roomName)
        {
            await _chat.SendMessage(dto, chatId, roomName);
            return Ok();
        }
        
    }
}
