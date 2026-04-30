using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using System.Security.Claims;

namespace Smart_Parking_Garage.Seeding;

public class DefaultUsersSeeding
{

    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext context)
    {
        // =====================
        // 1. Admin
        // =====================
        var admin = await userManager.FindByEmailAsync(DefaultUsers.AdminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin@123",
                Email = DefaultUsers.AdminEmail,
                EmailConfirmed = true,
                FirstName = "Smart Parking",
                LastName = "Admin"
            };

            await userManager.CreateAsync(admin, DefaultUsers.AdminPassword);
            await userManager.AddToRoleAsync(admin, DefaultRoles.Admin);
        }

        // =====================
        // 2. Garage Owners
        // =====================
        var ownersData = new[]
        {
            new { Email = DefaultUsers.Owner1Email, Name =DefaultUsers.Owner1Name,UserName=DefaultUsers.Owner1UserName,Password=DefaultUsers.Owner1Password },
            new { Email = DefaultUsers.Owner2Email, Name =DefaultUsers.Owner2Name,UserName=DefaultUsers.Owner2UserName,Password=DefaultUsers.Owner2Password  },
            new { Email = DefaultUsers.Owner3Email, Name =DefaultUsers.Owner3Name,UserName=DefaultUsers.Owner3UserName,Password=DefaultUsers.Owner3Password  }
        };

        foreach (var item in ownersData)
        {
            var owner = await userManager.FindByEmailAsync(item.Email);

            if (owner == null)
            {
                owner = new ApplicationUser
                {
                    UserName = item.UserName,
                    Email = item.Email,
                    EmailConfirmed = true,
                    FirstName = item.Name,
                    LastName = ""
                };

                await userManager.CreateAsync(owner, item.Password);
                await userManager.AddToRoleAsync(owner, DefaultRoles.GarageOwner);
            }

        }

        // =====================
        // . Garage Owner Permissions
        // =====================

            var role = await roleManager.FindByNameAsync(DefaultRoles.GarageOwner);

        if (role == null)
            return;

        var permissions = new List<string>
        {
            Permissions.GetGaragesStatus,
            Permissions.UpdateGarages,
            Permissions.GetParkingSlotsById,
        };

        foreach (var permission in permissions)
        {
            var exists = await roleManager.GetClaimsAsync(role);

            if (!exists.Any(c => c.Type == Permissions.Type && c.Value == permission))
            {
                await roleManager.AddClaimAsync(role,
                    new System.Security.Claims.Claim(Permissions.Type, permission));
            }
        }

    }



    

    public static async Task SeedPermissionsAsync(RoleManager<ApplicationRole> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync(DefaultRoles.Admin);
        var ownerRole = await roleManager.FindByNameAsync(DefaultRoles.GarageOwner);

        if (adminRole == null)
            throw new Exception("Admin role not found");

        if (ownerRole == null)
            throw new Exception("Owner role not found");

        var allPermissions = Permissions.GetAllPermissions();

        
        var garageOwnerNewPermissions = new List<string>
        {
            Permissions.GetGarageById
        };

        await AddPermissions(roleManager, adminRole, allPermissions);
        await AddPermissions(roleManager, ownerRole, garageOwnerNewPermissions);
    }
    private static async Task AddPermissions(
     RoleManager<ApplicationRole> roleManager,
     ApplicationRole role,
     IEnumerable<string> permissions)
    {
        var existing = await roleManager.GetClaimsAsync(role);

        foreach (var permission in permissions.Except(existing.Select(x => x.Value)))
        {
            await roleManager.AddClaimAsync(role,
                new Claim(Permissions.Type, permission));
        }
    }




}
