namespace Smart_Parking_Garage.Contracts.IOT;

public class DeviceCommandRequest
{
    public string CommandId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
