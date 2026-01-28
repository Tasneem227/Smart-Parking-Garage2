using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class Payment
{
    public int PaymentId { get; set; }


    public int ParkingSessionId { get; set; }

    public decimal Amount { get; set; }
    public string Status { get; set; }     // Paid / Failed / Pending
    public string PaymentMethod { get; set; }
    public DateTime TransactionTime { get; set; }

    // Navigation
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public ParkingSession? ParkingSession { get; set; }
}
