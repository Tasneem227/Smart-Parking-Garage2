namespace Smart_Parking_Garage.Errors;

public static class CarTypeErrors
{
    public static readonly Error UserCarTypeNotFound =
     new("CArType.CarType Not Found.", "User does not have this car type", StatusCodes.Status404NotFound);

}
