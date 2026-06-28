namespace Smart_Parking_Garage.Contracts.Booking;

public class BookingRequest {
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public int GarageId { get; set; }
    public string CarType { get; set; }
}
