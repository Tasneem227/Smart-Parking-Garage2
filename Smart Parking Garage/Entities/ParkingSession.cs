using Microsoft.EntityFrameworkCore;

namespace Smart_Parking_Garage.Entities;

public class ParkingSession
{
   
    public int ParkingSessionId { get; set; }

    public int BookingId { get; set; }
    public int ParkingSlotId { get; set; }

    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal ChargeAmount { get; set; }

    // Navigation
    public Booking? Booking { get; set; }
    public ParkingSlot? ParkingSlot { get; set; }
    public ICollection<Payment>? Payments { get; set; }
   // public CarTypeClassification CarTypeClassification { get; set; }
}
