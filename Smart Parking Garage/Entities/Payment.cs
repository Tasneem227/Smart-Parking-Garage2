using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int BookingId { get; set; }
    public int MockCardId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string? FailureReason { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime TransactionTime { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
    public Booking Booking { get; set; }
    public MockCard? MockCard { get; set; }
}