namespace Smart_Parking_Garage.Errors;

public static class ParkingSlotErrors
{
    public static readonly Error SlotNotFound =
        new("ParkingSLot.ParkingSLot Not Found.", "the ParkingSLot Is Not Found ", StatusCodes.Status404NotFound);

    public static readonly Error NoEmptySlotForCarType =
        new("ParkingSLot.ParkingSLot Not Found.", "there is no empty ParkingSLot for this carType ", StatusCodes.Status404NotFound);

    public static readonly Error SlotIsOccupied =
        new("ParkingSLot.ParkingSLot Is Occupied.", "the ParkingSLot Is Occupied", StatusCodes.Status404NotFound);
}
