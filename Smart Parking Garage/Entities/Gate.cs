namespace Smart_Parking_Garage.Entities;

public class Gate
{
    public int GateId { get; set; }
    public string GateType { get; set; }   // Entrance / Exit
    public string DeviceId { get; set; }
    public int? GarageId { get; set; }
    public Garage Garage { get; set; }
    public string Status { get; set; }

    // Navigation
  //  public ICollection<GateLog> GateLogs { get; set; }
}
