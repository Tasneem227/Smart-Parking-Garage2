namespace Smart_Parking_Garage.Contracts.Notification;

public record NotificationResponse
(
     int NotificationId,
     string Title,
     string Message,
     string Type,
     bool IsRead,
     DateTime CreatedAt
);