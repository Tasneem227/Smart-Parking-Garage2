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
public class ChatbotController : ControllerBase
{
    private readonly AiChatService _aiChatService;

    public ChatbotController(AiChatService aiChatService)
    {
        _aiChatService = aiChatService;
    }
    [HasPermission(Permissions.SendChatbotMessage)]
    [HttpPost("message")]
    public async Task<IActionResult> SendMessage(
        [FromBody] ChatbotMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message is required");

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("User not authenticated");

        string userId = userIdClaim.Value;

        var reply = await _aiChatService.SendAsync(
            userId,
            request.Message,
            request.Latitude,
            request.Longitude
        );

        return Ok(new ChatbotResponse
        {
            Message = reply
        });
    }
}
