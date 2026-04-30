using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace Smart_Parking_Garage.Entities;

public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
