namespace Smart_Parking_Garage.Contracts.Garage;

public class GarageCreate
{
    public string Name { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int TotalSlots { get; set; }

}
