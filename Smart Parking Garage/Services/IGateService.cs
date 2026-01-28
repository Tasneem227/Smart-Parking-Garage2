namespace Smart_Parking_Garage.Services;

public interface IGateService
{
    Task<IEnumerable<Gate>> GetAllGatesAsync(CancellationToken cancellationToken = default);
    Task<Gate?> GetGateByIdAsync(int id , CancellationToken cancellationToken = default);
    Task<Gate> CreateGateAsync(Gate gate , CancellationToken cancellationToken = default);
    Task<bool> UpdateGateAsync(int id, Gate gate , CancellationToken cancellationToken = default);
    Task<bool> UpdateGateStatusAsync(int id, string status, CancellationToken cancellationToken = default);
    Task<bool> DeleteGateAsync(int id, CancellationToken cancellationToken = default);

}
