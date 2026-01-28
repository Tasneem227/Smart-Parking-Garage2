namespace Smart_Parking_Garage.Contracts.Gate;

public record GateResponse
    (
    int GateId,
    string GateType ,
    string DeviceId,
    string Status
    );
