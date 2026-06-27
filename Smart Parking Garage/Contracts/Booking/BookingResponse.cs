namespace Smart_Parking_Garage.Contracts.Booking;

public class  BookingResponse
{
    public int BookingId { get; set; }
    public string? UserId { get; set; }

    public DateTime BookingStart { get; set; }
    public DateTime? BookingEnd { get; set; }
    public decimal? Price { get; set; }
    public string Status {  get; set; }
    public bool PriorityApplied { get; set; }
    public string? SlotNumber { get; set; }
    public int? GarageId { get; set; }
}
