namespace Smart_Parking_Garage.Errors;

public class BookingErrors
{
    public static readonly Error BookingNotFound =
      new("Booking.Booking Not Found.", "Booking not found", StatusCodes.Status404NotFound);

    public static readonly Error NoValidBookingToOpenEntryGate =
     new("Booking.Booking Not Found.", "you can not open Entry gate , No valid booking found.", StatusCodes.Status404NotFound);

    public static readonly Error NoValidBookingToOpenExitGate =
    new("Booking.Booking Not Found.", "you can not open Exit gate , No valid booking found.", StatusCodes.Status404NotFound);

    public static readonly Error CancelledBooking =
      new("Booking.Cancelled Booking  .", "Booking is already cancelled", StatusCodes.Status400BadRequest);

    public static readonly Error BookingAlreadyStarted =
     new("Booking.AlreadyStarted  .", "Booking has already started and cannot be cancelled", StatusCodes.Status400BadRequest);
}
