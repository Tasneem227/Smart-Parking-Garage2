namespace Smart_Parking_Garage.Contracts.Payment;

public record PaymentRequest(
    int BookingId,
    string CardNumber,
    string ExpiryDate,
    string CVV
);