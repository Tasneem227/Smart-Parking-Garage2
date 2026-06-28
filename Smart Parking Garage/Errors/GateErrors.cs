namespace Smart_Parking_Garage.Errors;

public class GateErrors
{
    public static readonly Error GateNotFound =
     new("Gate.Gate Not Found.", "the Gate With this Id Is Not Found ", StatusCodes.Status404NotFound);

    public static readonly Error GarageNotFound =
    new("Garage.Garage Not Found.", "This garage not found so we cannot assign any gate to it", StatusCodes.Status404NotFound);


}
