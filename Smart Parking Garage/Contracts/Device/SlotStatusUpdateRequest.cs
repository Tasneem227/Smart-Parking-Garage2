namespace Smart_Parking_Garage.Contracts.Device;

public sealed class SlotStatusUpdateRequest
{
    public string DeviceId { get; set; } = "raspberry-01";

    public int slotId { get; set; }

    public string Status { get; set; } = default!;

    public DateTimeOffset Timestamp { get; set; }

}
