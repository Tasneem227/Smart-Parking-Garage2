using Microsoft.AspNetCore.Identity;

namespace Smart_Parking_Garage.Entities;

public class ApplicationUser : IdentityUser
{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PriorityLevel { get; set; }  

    // Navigation
    public ICollection<Booking>? Bookings { get; set; }
    public ICollection<Payment>? Payments { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];

}
