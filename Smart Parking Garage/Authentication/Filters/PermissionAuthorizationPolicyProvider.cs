using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Smart_Parking_Garage.Authentication.Filters;

// Custom Policy Provider that dynamically creates authorization policies
public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    // Store the AuthorizationOptions to allow adding new policies at runtime
    private readonly AuthorizationOptions _authorizationOptions = options.Value;

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // محاولة الحصول على policy موجود بالفعل (registered policy)
        var policy = await base.GetPolicyAsync(policyName);

        // If the policy already exists, return it مباشرة
        if (policy is not null)
            return policy;

        // If the policy does NOT exist:
        // create a new policy dynamically using the policy name as a permission
        var permissionPolicy = new AuthorizationPolicyBuilder()
            // Add a custom requirement (PermissionRequirement)
            // using policyName as the required permission
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        // Add the newly created policy to the AuthorizationOptions
        // so it can be reused next time بدون إعادة إنشائه
        _authorizationOptions.AddPolicy(policyName, permissionPolicy);

        // Return the dynamically created policy
        return permissionPolicy;
    }
}