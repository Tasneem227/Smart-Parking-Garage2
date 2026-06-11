namespace Smart_Parking_Garage.Contracts.Device;

public sealed class SlotStatusUpdateRequest
{
    public string DeviceId { get; set; } = "raspberry-01";

    public int slotId { get; set; } 

    public bool? IsOccupied { get; set; }

    public DateTimeOffset Timestamp { get; set; }

}
