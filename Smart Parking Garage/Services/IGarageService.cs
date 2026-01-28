using Smart_Parking_Garage.Contracts.Garage;

namespace Smart_Parking_Garage.Services;

public interface IGarageService
{
    Task<List<Garage>> GetAllAsync();
    Task<Garage?> GetByIdAsync(int id);
    Task CreateAsync(GarageCreate dto);
    Task<bool> UpdateAsync(int id, GarageCreate dto);
    Task<bool> DeleteAsync(int id);

    Task<List<GarageLocation>> GetAllGarageLocationsAsync(CancellationToken cancellationToken);
}
