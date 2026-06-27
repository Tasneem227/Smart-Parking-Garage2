



using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Authentication.Filters;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Chatbot;
using Smart_Parking_Garage.Services;
using System.Security.Claims;

namespace Smart_Parking_Garage.Controllers;

[ApiController]
[Route("api/chatbot")]
[Authorize]
public class ChatBotController : ControllerBase
{
    private readonly AiChatService _aiChatService;

    public ChatBotController(AiChatService aiChatService)
    {
        _aiChatService = aiChatService;
    }

    [HasPermission(Permissions.SendChatbotMessage)]
    [HttpPost("message")]
    public async Task<IActionResult> SendMessage(
        [FromBody] ChatbotMessageRequest request)
    {
        var token = Request.Headers.Authorization
        .ToString()
        .Replace("Bearer ", "");

        var reply = await _aiChatService.SendAsync(request,token);

        return Ok(new ChatbotResponse
        {
            Message = reply
        });
    }
}
