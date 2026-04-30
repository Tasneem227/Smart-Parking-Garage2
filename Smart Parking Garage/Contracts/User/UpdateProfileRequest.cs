namespace Smart_Parking_Garage.Contracts.User;

public record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string? UserName,
    string? PhoneNumber
);