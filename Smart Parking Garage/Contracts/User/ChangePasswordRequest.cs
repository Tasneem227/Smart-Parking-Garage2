namespace Smart_Parking_Garage.Contracts.User;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);