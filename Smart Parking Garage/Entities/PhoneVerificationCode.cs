namespace Smart_Parking_Garage.Entities;

public class PhoneVerificationCode
{

    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public int Attempts { get; set; }
}
