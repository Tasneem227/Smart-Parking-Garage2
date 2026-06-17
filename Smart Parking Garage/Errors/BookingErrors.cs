namespace Smart_Parking_Garage.Errors;

public class BookingErrors
{
    public static readonly Error NoValidBookingToOpenEntryGate =
     new("Booking.Booking Not Found.", "you can not open Entry gate , No valid booking found.", StatusCodes.Status404NotFound);

    public static readonly Error NoValidBookingToOpenExitGate =
    new("Booking.Booking Not Found.", "you can not open Exit gate , No valid booking found.", StatusCodes.Status404NotFound);

}
