using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class ParkingSlot
{
    public int ParkingSlotId { get; set; }
    public string SlotNumber { get; set; }  
    public string SlotType { get; set; }
    public bool IsOccupied { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerHour { get; set; }

    public int GarageId { get; set; }
    public Garage? Garage { get; set; }
    public Sensor? Sensor { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
    public ICollection<ParkingSession>? ParkingSessions { get; set; }
}
