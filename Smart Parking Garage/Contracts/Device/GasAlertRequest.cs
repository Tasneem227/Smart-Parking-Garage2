namespace Smart_Parking_Garage.Contracts.Device;

public record GasAlertRequest(
     string DeviceId,
    string Type,
    DateTime Timestamp
    );
