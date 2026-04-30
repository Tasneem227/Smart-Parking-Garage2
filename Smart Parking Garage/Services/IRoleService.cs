using Smart_Parking_Garage.Contracts.Roles;

namespace Smart_Parking_Garage.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
}
