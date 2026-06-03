using Microsoft.EntityFrameworkCore;

namespace Smart_Parking_Garage.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SendAsync(string userId, string title, string message, string type)
    {
        var notification = new Notification
        {
            ApplicationUserId = userId,
            Title = title,
            Message = message,
            Type = type
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }


    public async Task<List<Notification>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(n => n.ApplicationUserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);


    }
    public async Task<bool> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);

        if (notification == null)
            return false;

        notification.IsRead = true;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int notificationId, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);

        if (notification == null)
            return false;

        _context.Notifications.Remove(notification);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .CountAsync(n => n.ApplicationUserId == userId && !n.IsRead, cancellationToken);
    }

    public async Task<bool> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => n.ApplicationUserId == userId && !n.IsRead)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
            return false;

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAllAsync(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => n.ApplicationUserId == userId)
            .ToListAsync(cancellationToken);

        _context.Notifications.RemoveRange(notifications);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<Notification>> GetReadNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(n => n.ApplicationUserId == userId && n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(n => n.ApplicationUserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

}