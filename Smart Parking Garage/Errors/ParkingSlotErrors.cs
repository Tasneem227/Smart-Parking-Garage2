namespace Smart_Parking_Garage.Errors;

public static class ParkingSlotErrors
{
        public static readonly Error SlotNotFound =
        new("ParkingSLot.ParkingSLot Not Found.", "the ParkingSLot Is Not Found ", StatusCodes.Status404NotFound);
}
