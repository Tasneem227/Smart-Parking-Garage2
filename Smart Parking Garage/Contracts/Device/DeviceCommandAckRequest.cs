namespace Smart_Parking_Garage.Contracts.IOT;

public class DeviceCommandAckRequest
{
    public string DeviceId { get; set; } = string.Empty;

    public string CommandId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? Error { get; set; }

    public DateTimeOffset TimeStamp { get; set; }
}