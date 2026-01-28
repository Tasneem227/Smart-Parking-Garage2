namespace Smart_Parking_Garage.Services;

public interface IBookingService
{
    Task<BookingResponse> AddBooking(BookingRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookingResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<BookingResponse>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<BookingResponse> UpdateBookingTimeAsync(int id, updateBookingTimeRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateBookingStatusAsync(int id, UpdateBookingStatusRequest status, CancellationToken cancellationToken);



}
