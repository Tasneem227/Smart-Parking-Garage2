using Smart_Parking_Garage.Contracts.Gate;

namespace Smart_Parking_Garage.Services;

public interface IGateService
{
    Task<Result<IEnumerable<GateResponse>>> GetAllGatesAsync(CancellationToken cancellationToken = default);
    Task<Result<GateResponse>> GetGateByIdAsync(int id , CancellationToken cancellationToken = default);
    Task<Result<GateResponse>> CreateGateAsync(GateRequest gate, CancellationToken cancellationToken = default);
    Task<Result> UpdateGateAsync(int id, UpdateGateRequest gate , CancellationToken cancellationToken = default);
    Task<Result> DeleteGateAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateGateStatusAsync(int id,CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<GateResponse>>> GetGatesByGarageIdAsync(int garageId, CancellationToken cancellationToken = default);
}
