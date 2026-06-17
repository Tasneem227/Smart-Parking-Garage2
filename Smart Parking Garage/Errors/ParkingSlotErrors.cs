namespace Smart_Parking_Garage.Errors;

public static class ParkingSlotErrors
{
    public static readonly Error GarageNotFound =
       new("Garage.Garage Not Found.", "This garage not found so we cannot assign any parking slot to it", StatusCodes.Status404NotFound);



    public static readonly Error SlotNotFound =
        new("ParkingSLot.ParkingSLot Not Found.", "the ParkingSLot Is Not Found ", StatusCodes.Status404NotFound);

    public static readonly Error AvailableSlotsNotFound =
      new("ParkingSLot.Available ParkingSLot Not Found.", "No Available parking slots ", StatusCodes.Status404NotFound);

    public static readonly Error SlotsNotFound =
      new("ParkingSLot.ParkingSLot Not Found.", "this garage dosenot have any slots ", StatusCodes.Status404NotFound);

}
