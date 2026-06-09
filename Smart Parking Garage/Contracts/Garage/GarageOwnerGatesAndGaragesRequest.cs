namespace Smart_Parking_Garage.Contracts.Garage;

public record GarageOwnerGatesAndGaragesRequest(
    int garageId,
    IEnumerable<int> GateIds
    );
