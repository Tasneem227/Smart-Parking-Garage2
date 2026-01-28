namespace Smart_Parking_Garage.Contracts.ParkingSlot;

public record UpdateParkingSlotRequest
    (
    string SlotNumber,
    string SlotType,
    int? SensorId
    )
{
}
