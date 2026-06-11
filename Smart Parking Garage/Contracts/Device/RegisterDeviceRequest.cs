namespace Smart_Parking_Garage.Contracts.Device;

public sealed class RegisterDeviceRequest
{
    public string DeviceId { get; set; } = "raspberry-01";

    public int GarageId { get; set; } = 1;

    public int SlotsCount { get; set; }

    public bool HasCamera { get; set; }

    public bool HasEnvSensors { get; set; }

    public bool HasEntryGate { get; set; }

    public bool HasExitGate { get; set; }

    public DateTimeOffset TimeStamp { get; set; } 
}