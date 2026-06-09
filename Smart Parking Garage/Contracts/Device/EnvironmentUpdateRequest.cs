namespace Smart_Parking_Garage.Contracts.Device;

public sealed class EnvironmentUpdateRequest
{
    public string DeviceId { get; set; } = "raspberry-01";

    public decimal? Temperature { get; set; }

    public decimal? Humidity { get; set; }

    public bool? Gas { get; set; }

    public DateTimeOffset Timestamp { get; set; } 
}