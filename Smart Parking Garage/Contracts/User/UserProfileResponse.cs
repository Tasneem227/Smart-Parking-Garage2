namespace Smart_Parking_Garage.Contracts.User;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNumber
);
