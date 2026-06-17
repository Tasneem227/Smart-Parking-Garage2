using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.Notification;
using System.Collections.Generic;

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


    public async Task<Result<IEnumerable<NotificationResponse>>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
        var UserNotifications = await _context.Notifications .Where(n => n.ApplicationUserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);

        return Result.Success(UserNotifications.Adapt<IEnumerable<NotificationResponse>>());

    }
    public async Task<Result> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken)
    {
        
        var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId , cancellationToken);
        if (notification is null)
            return Result.Failure(NotificationErrors.NotificationNotFound);
        if (notification.IsRead)
            return Result.Failure(NotificationErrors.NotificationAlreadyRead);


        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int notificationId, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);

        if (notification is null)
            return Result.Failure(NotificationErrors.NotificationNotFound);

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken)
    {
        var UnreadNotificationCount = await _context.Notifications
            .CountAsync(x => x.ApplicationUserId == userId && !x.IsRead, cancellationToken);
        return Result.Success(UnreadNotificationCount);
    }

    public async Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications.Where(n => n.ApplicationUserId == userId && !n.IsRead).ToListAsync(cancellationToken);

        if (!notifications.Any()) 
            return Result.Failure(NotificationErrors.NoUnreadNotifications);

        foreach (var notification in notifications)
        {

            notification.IsRead = true;
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAllAsync(string userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications.Where(n => n.ApplicationUserId == userId).ToListAsync(cancellationToken);
        if (!notifications.Any())
            return Result.Failure(NotificationErrors.NoNotificationsToDelete);

        _context.Notifications.RemoveRange(notifications);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<NotificationResponse>>> GetReadNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
      var ReadNotification = await _context.Notifications .Where(x => x.ApplicationUserId == userId && x.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);

        return Result.Success(ReadNotification.Adapt<IEnumerable<NotificationResponse>>());
    }

    public async Task<Result<IEnumerable<NotificationResponse>>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken)
    {
      var UnreadNotifications =  await _context.Notifications.Where(n => n.ApplicationUserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
        return Result.Success(UnreadNotifications.Adapt<IEnumerable<NotificationResponse>>());
    }

}