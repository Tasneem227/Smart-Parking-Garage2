namespace Smart_Parking_Garage.Contracts.Authentication;

public record registerRequest(
    string Email,
    string Password,
    string UserName,
    string FirstName,
    string LastName
    );
