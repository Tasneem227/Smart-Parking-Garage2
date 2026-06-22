using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Notification;
using System.Security.Claims;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetUserNotifications( CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.GetUserNotificationsAsync(userId, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut("mark-as-read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAsReadAsync(id, cancellationToken);
        if (!result.IsSuccess)
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: result.Error.StatusCode);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _notificationService.DeleteAsync(id, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : NoContent();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount( CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.GetUnreadCountAsync(userId, cancellationToken);
        return Ok(result.Value);
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead( CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.MarkAllAsReadAsync(userId, cancellationToken);

        return (result.IsSuccess) ? NoContent() : NotFound(result.Error);
    }

    [HttpDelete("delete_All")]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.DeleteAllAsync(userId, cancellationToken);

        return (result.IsSuccess) ? NoContent() : NotFound(result.Error);
    }

    [HttpGet("get-all-read")]
    public async Task<IActionResult> GetReadNotifications(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.GetReadNotificationsAsync(userId, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("get-all-unread")]
    public async Task<IActionResult> GetUnreadNotifications(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _notificationService.GetUnreadNotificationsAsync(userId, cancellationToken);
        return Ok(result.Value);
    }
}