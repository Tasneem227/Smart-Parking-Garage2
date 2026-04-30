using Microsoft.AspNetCore.Identity;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;

namespace Smart_Parking_Garage.Persistence.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        //builder.HasData(new IdentityUserRole<string>
        //{
        //    UserId = DefaultUsers.AdminId,
        //    RoleId = DefaultRoles.AdminRoleId
        //});
    }
}
