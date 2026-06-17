namespace Smart_Parking_Garage.Contracts.Authentication;


public record resetPasswordRequest(
    string Email,
    string Code,
    string NewPassword
);