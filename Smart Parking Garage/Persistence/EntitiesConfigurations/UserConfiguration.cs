
using Microsoft.AspNetCore.Identity;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;

namespace Smart_Parking_Garage.Persistence.EntitiesConfigurations;

public class UserConfiguration:IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("ApplicationUserId");
        

        //var passwordHasher = new PasswordHasher<ApplicationUser>();
        //var pass = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword);
        //builder.HasData(new ApplicationUser
        //{
        //    Id = DefaultUsers.AdminId,

        //    FirstName = "Smart Parking Garage",
        //    LastName = "Admin",
        //    UserName = DefaultUsers.AdminEmail,
        //    NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
        //    Email = DefaultUsers.AdminEmail,
        //    NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
        //    SecurityStamp = DefaultUsers.AdminSecurityStamp,
        //    ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
        //    EmailConfirmed = true,
        //    PasswordHash = "AQAAAAIAAYagAAAAEC5+G1lhEzrq4NmN5g/8zOzTJYZ0ssYWPsW1QybpNlMsJjhPHMPnVICs2iLxLhvH8Q=="

        //});
    }
    
   
    
}

