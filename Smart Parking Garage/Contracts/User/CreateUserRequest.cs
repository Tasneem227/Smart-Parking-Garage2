namespace Smart_Parking_Garage.Contracts.User;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string PhoneNumber,
    string Email,
    string Password,
    IList<string> Roles
);