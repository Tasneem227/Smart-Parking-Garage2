using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class Notification
{
    public int NotificationId { get; set; }


    public string Message { get; set; }
    
    public bool IsRead { get; set; }

    // Navigation
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
