namespace Smart_Parking_Garage.Contracts.Authentication;

public class ConfirmEmailRequest
{
   public string UserId { get; set; } =string.Empty;
   public string code { get; set; } = string.Empty;
}