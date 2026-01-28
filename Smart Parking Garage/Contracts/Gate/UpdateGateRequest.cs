namespace Smart_Parking_Garage.Contracts.Gate;

public record UpdateGateRequest(
 string? GateType ,
 string? DeviceId ,
string? Status 
    );