namespace Smart_Parking_Garage.Contracts.SensorReading;

public class UpdateSensorJson
{
    public bool Slot1 { get; set; }
    public bool Slot2 { get; set; }
    public bool Slot3 { get; set; }
    public string EntryGate { get; set; }
    public string ExitGate { get; set; }
}
