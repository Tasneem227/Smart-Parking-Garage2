using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Notification;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("get-all/{userId}")]
    public async Task<IActionResult> GetUserNotifications(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(userId, cancellationToken);

        var response = notifications.Adapt<IEnumerable<NotificationResponse>>();

        return Ok(response);
    }

    [HttpPut("mark-as-read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAsReadAsync(id, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _notificationService.DeleteAsync(id, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("unread-count/{userId}")]
    public async Task<IActionResult> GetUnreadCount(string userId, CancellationToken cancellationToken)
    {
        var count = await _notificationService.GetUnreadCountAsync(userId, cancellationToken);

        return Ok(new { count });
    }

    [HttpPut("read-all/{userId}")]
    public async Task<IActionResult> MarkAllAsRead(string userId, CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAllAsReadAsync(userId, cancellationToken);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("delete_All/{userId}")]
    public async Task<IActionResult> DeleteAll(string userId, CancellationToken cancellationToken)
    {
        await _notificationService.DeleteAllAsync(userId, cancellationToken);

        return NoContent();
    }

    [HttpGet("get-all-read/{userId}")]
    public async Task<IActionResult> GetReadNotifications(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _notificationService.GetReadNotificationsAsync(userId, cancellationToken);

        var response = notifications.Adapt<IEnumerable<NotificationResponse>>();

        return Ok(response);
    }

    [HttpGet("get-all-unread/{userId}")]
    public async Task<IActionResult> GetUnreadNotifications(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId, cancellationToken);

        var response = notifications.Adapt<IEnumerable<NotificationResponse>>();

        return Ok(response);
    }
}