namespace Smart_Parking_Garage.Contracts.Booking;

public record BookingResponse
(
    int BookingId,
    DateTime BookingStart,
    DateTime? BookingEnd,
    decimal? Price,
    string Status,
    bool PriorityApplied,
    int ParkingSlotId,
    int ? GarageId,
    string? ApplicationUserId
);
