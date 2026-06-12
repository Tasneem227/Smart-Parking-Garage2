namespace Smart_Parking_Garage.Errors;

public class GateErrors
{
    public static readonly Error GateNotFound =
     new("Gate.Gate Not Found.", "the Gate With this Id Is Not Found ", StatusCodes.Status404NotFound);

}
