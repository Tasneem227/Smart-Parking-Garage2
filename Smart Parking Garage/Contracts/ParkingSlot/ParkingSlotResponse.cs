namespace Smart_Parking_Garage.Contracts.ParkingSlot;

public record ParkingSlotResponse(
    int ParkingSlotId,
    string SlotNumber,
    string SlotType,
    bool IsOccupied,
    DateTime LastUpdated,
    int? SensorId
    )
{

}
