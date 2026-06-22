using Smart_Parking_Garage.Contracts.Notification;

namespace Smart_Parking_Garage.Services;

public interface INotificationService
{
    Task SendAsync(string userId, string title, string message, string type);
    Task<Result<IEnumerable<NotificationResponse>>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken);
    Task<Result> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int notificationId, CancellationToken cancellationToken);
    Task<Result<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken);
    Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken);
    Task<Result> DeleteAllAsync(string userId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<NotificationResponse>>> GetReadNotificationsAsync(string userId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<NotificationResponse>>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken);


}
