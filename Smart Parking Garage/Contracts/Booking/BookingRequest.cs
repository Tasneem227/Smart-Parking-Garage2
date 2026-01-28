namespace Smart_Parking_Garage.Contracts.Booking;

public class BookingRequest {
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public bool PriorityApplied { get; set; }
    public string SlotNumber { get; set; }

}
