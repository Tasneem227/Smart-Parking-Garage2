namespace Smart_Parking_Garage.Contracts.Device;

public sealed record RegisterDeviceResponse(
    string deviceId,
    int garageId,
    string message
 );