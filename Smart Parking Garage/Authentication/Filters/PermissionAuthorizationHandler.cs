using Microsoft.AspNetCore.Authorization;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;


namespace Smart_Parking_Garage.Authentication.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        
        //var user = context.User.Identity;

        //if (user is null || !user.IsAuthenticated)
        //    return;

        //var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type);

        //if (!hasPermission)
        //    return;

        // Check if the user is authenticated (logged in)
        // OR if the user does NOT have the required permission in their claims
        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(x =>
                x.Value == requirement.permission &&      // matches required permission value
                x.Type == Permissions.Type))              // matches the permission claim type
        {
            // If any of the above conditions fail, do nothing
            // Authorization will fail automatically
            return;
        }

        // If the user is authenticated AND has the required permission
        // mark the requirement as successfully satisfied
        context.Succeed(requirement);

        // End of method
        return;
    }
}