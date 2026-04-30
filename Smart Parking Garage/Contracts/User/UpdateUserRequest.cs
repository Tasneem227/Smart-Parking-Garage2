namespace Smart_Parking_Garage.Contracts.User;

public record UpdateUserRequest(
    string? FirstName,
    string? LastName,
    string? UserName,
    string? PhoneNumber,
    string? Email,
    IList<string>? Roles
);