namespace Smart_Parking_Garage.Contracts.Garage;

public class GarageLocation
{
    public int GarageId { get; set; }
    public string GarageName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<GarageSlots> Slots { get; set; }
}