namespace Smart_Parking_Garage.Errors;

<<<<<<< HEAD
public class BookingErrors
{
    public static readonly Error NoValidBookingToOpenEntryGate =
     new("Booking.Booking Not Found.", "you can not open Entry gate , No valid booking found.", StatusCodes.Status404NotFound);

    public static readonly Error NoValidBookingToOpenExitGate =
    new("Booking.Booking Not Found.", "you can not open Exit gate , No valid booking found.", StatusCodes.Status404NotFound);
=======
public static class BookingErrors
{
>>>>>>> f8cc2ad81a0b1d12d7f2c77d1a320e0f9a9684ff

}
