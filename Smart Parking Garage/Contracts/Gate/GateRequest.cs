namespace Smart_Parking_Garage.Contracts.Gate;

public record GateRequest(
    string GateType,
    string DeviceId,
    string Status
    );

