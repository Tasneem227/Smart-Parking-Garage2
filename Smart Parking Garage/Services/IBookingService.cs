namespace Smart_Parking_Garage.Services;

public interface IBookingService
{
    Task<Result<BookingResponse>> AddBooking(BookingRequest request, string userid, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookingResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BookingResponse>>> GetBookingsByGarageIdAsync(int garageId,CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<BookingResponse>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<BookingResponse> UpdateBookingTimeAsync(int id, updateBookingTimeRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateBookingStatusAsync(int id, UpdateBookingStatusRequest status, CancellationToken cancellationToken);
    Task DeleteByLastBookingByUserId(string userid, CancellationToken cancellationToken = default);
    Task<Result<Booking>> GetCurrentBookingForGateAsync(string userId,CancellationToken cancellationToken = default);
    Task<Result<Booking>> GetCurrentBookingForExitGateAsync(string userId,CancellationToken cancellationToken = default);
    Task<Result> CancelBookingAsync(int bookingId);
}
