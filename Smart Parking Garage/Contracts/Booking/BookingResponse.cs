namespace Smart_Parking_Garage.Contracts.Booking;

public record BookingResponse
(
    int BookingId,
    DateTime BookingStart,
    DateTime BookingEnd,
    string Status,
    bool PriorityApplied,
    int ParkingSlotId,
    string? ApplicationUserId
);
