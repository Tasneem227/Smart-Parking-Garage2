namespace Smart_Parking_Garage.Services;

public interface INotificationService
{
    Task SendAsync(string userId, string title, string message, string type);
    Task<List<Notification>> GetUserNotificationsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int notificationId, CancellationToken cancellationToken);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken);
    Task<bool> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken);
    Task<bool> DeleteAllAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetReadNotificationsAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken);


}
