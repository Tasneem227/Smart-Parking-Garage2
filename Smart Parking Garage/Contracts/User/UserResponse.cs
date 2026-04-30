namespace Smart_Parking_Garage.Contracts.User;

public record UserResponse(
    string Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string PhoneNumber,
    bool IsDisabled,
    IEnumerable<string> Roles
);