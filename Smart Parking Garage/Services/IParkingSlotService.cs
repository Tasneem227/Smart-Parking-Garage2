namespace Smart_Parking_Garage.Services;

public interface IParkingSlotService
{
    Task<Result<IEnumerable<ParkingSlotResponse>>> GetAllSlotsAsync(CancellationToken cancellationToken = default);
    Task<Result<ParkingSlotResponse>> GetSlotByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ParkingSlotResponse>> CreateSlotAsync(ParkingSlotRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateSlotAsync(int id, UpdateParkingSlotRequest request, CancellationToken cancellationToken = default);
   // Task<Result> DeleteSlotAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ParkingSlotResponse>>> GetAvailableSlotsAsync(CancellationToken cancellationToken = default);
    Task<Result> ToggleOccupancyAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ParkingSlotResponse>>> GetSlotsByGarageIdAsync( int garageId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ParkingSlotResponse>>> GetAvailableSlotsByGarageIdAsync(int garageId,CancellationToken cancellationToken = default);
}
