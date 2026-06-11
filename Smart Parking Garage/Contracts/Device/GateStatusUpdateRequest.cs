namespace Smart_Parking_Garage.Contracts.Device;

public sealed class GateStatusUpdateRequest
{
    public string DeviceId { get; set; } = default!;

    public string Gate { get; set; } = default!;

    public string Status { get; set; } = default!;

    public DateTimeOffset Timestamp { get; set; }
}