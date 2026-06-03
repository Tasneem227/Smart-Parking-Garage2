using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class Notification
{
    public int NotificationId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty; // Booking, Slot, Gate

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }
}