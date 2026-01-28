namespace Smart_Parking_Garage.Services;

public interface IParkingSlotService
{
    Task<IEnumerable<ParkingSlot>> GetAllSlotsAsync(CancellationToken cancellationToken = default);

    Task<ParkingSlot?> GetSlotByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ParkingSlot> CreateSlotAsync(ParkingSlot request, CancellationToken cancellationToken = default);

    Task<bool> UpdateSlotAsync(int id, ParkingSlot request, CancellationToken cancellationToken = default);

    Task<bool> DeleteSlotAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<ParkingSlot>> GetAvailableSlotsAsync(CancellationToken cancellationToken = default);

    Task<bool> ToggleOccupancyAsync(int id, CancellationToken cancellationToken = default);
}
