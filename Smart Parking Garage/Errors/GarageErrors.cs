namespace Smart_Parking_Garage.Errors;

public static class GarageErrors
{
    public static readonly Error GarageNotFound =
      new("Garage.Garage Not Found.", "the Garage With this Id Is Not Found ", StatusCodes.Status404NotFound);

}
