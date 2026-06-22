namespace Smart_Parking_Garage.Errors;

public static class NotificationErrors
{
    public static readonly Error NotificationNotFound =
   new( "Notification.NotFound", "The notification with this ID was not found.",StatusCodes.Status404NotFound);

    public static readonly Error NoUnreadNotifications =
   new( "Notification.NoUnreadNotifications", "There are no unread notifications.",StatusCodes.Status404NotFound);

    public static readonly Error NoNotificationsToDelete =
   new( "Notification.NoNotificationsToDelete", "There are no notifications to delete.",StatusCodes.Status404NotFound);

    public static readonly Error NotificationAlreadyRead =
   new( "Notification.AlreadyRead", "This notification has already been read.",StatusCodes.Status400BadRequest);
}