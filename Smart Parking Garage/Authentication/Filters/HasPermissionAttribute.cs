using Microsoft.AspNetCore.Authorization;

namespace Smart_Parking_Garage.Authentication.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}