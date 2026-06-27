namespace Smart_Parking_Garage.Contracts.Booking;

public class BookingRequest {
    public DateTime BookingStart { get; set; }= DateTime.UtcNow.AddHours(3);
    public DateTime BookingEnd { get; set; }=DateTime.UtcNow.AddHours(3);
    public int GarageId { get; set; }
    public string CarType { get; set; }
}
