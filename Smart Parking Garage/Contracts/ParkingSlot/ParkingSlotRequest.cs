namespace Smart_Parking_Garage.Contracts.ParkingSlot;

public record ParkingSlotRequest(
    string SlotNumber,
    string SlotType,
    bool IsOccupied,
    DateTime LastUpdated,
    int? SensorId
    )
{

}

 