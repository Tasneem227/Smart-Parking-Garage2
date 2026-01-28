namespace Smart_Parking_Garage.Contracts.Garage;

public class GarageRequest
{
    public int GarageId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int TotalSlots { get; set; }
    public int AvailableSlots { get; set; }
    public bool IsActive { get; set; }

}
