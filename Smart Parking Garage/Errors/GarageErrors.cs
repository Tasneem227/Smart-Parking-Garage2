namespace Smart_Parking_Garage.Errors;

public static class GarageErrors
{
    public static readonly Error GarageNotFound =
      new("Garage.Garage Not Found.", "Garage not found ", StatusCodes.Status404NotFound);

}
