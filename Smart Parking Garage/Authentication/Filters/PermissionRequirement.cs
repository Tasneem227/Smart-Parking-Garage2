using Microsoft.AspNetCore.Authorization;

namespace Smart_Parking_Garage.Authentication.Filters;

public class PermissionRequirement(string Permission): IAuthorizationRequirement
{
    public string permission = Permission;

}
