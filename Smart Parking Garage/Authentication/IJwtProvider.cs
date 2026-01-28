using Smart_Parking_Garage.Entities;

namespace Smart_Parking_Garage.Authentication;

public interface IJwtProvider
{
    public string? ValidateToken(string token); 
    (string token,int expiresIn) GenerateToken(ApplicationUser user);
}
