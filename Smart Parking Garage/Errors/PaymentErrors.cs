namespace Smart_Parking_Garage.Errors;

public static class PaymentErrors
{
    public static readonly Error BookingNotFound =
        new( "Payment.Booking Not Found.", "The requested booking could not be found.",StatusCodes.Status404NotFound);

    public static readonly Error CardNotFound =
        new( "Payment.Card Not Found.", "The card information provided is incorrect.",StatusCodes.Status404NotFound);

    public static readonly Error CardBlocked =
        new( "Payment.Card Is Blocked.", "This card is blocked and cannot be used for payments.",StatusCodes.Status400BadRequest);

    public static readonly Error InsufficientBalance =
        new( "Payment.Insufficient Balance.", "The available balance on this card is not enough to pay for this booking.",StatusCodes.Status400BadRequest);

    public static readonly Error AlreadyPaid =
        new( "Payment.Already Paid.", "This booking has already been paid.",StatusCodes.Status400BadRequest);
}