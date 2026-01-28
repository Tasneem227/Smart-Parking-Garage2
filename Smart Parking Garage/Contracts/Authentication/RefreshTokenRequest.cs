namespace Smart_Parking_Garage.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
    );

