namespace Smart_Parking_Garage.Contracts.Booking;

public record updateBookingTimeRequest
(
    DateTime BookingStart,
    DateTime BookingEnd
);
