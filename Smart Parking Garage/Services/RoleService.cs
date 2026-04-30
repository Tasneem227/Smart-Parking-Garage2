using Microsoft.AspNetCore.Identity;
using Smart_Parking_Garage.Contracts.Roles;

namespace Smart_Parking_Garage.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager):IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default) =>
    await _roleManager.Roles
        .Where(x => (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
        .ProjectToType<RoleResponse>()
        .ToListAsync(cancellationToken);

}
