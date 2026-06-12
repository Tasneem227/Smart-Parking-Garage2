using System.Text.Json.Serialization;

namespace Smart_Parking_Garage.Contracts.Authentication;

public class AuthResponse
{
   public string Id { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? garageId { get; set; }
    public string Token { get; set; }
    public int ExpiresIn { get; set; } 
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
