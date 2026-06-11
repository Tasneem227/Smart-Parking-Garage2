namespace Smart_Parking_Garage.Errors;

public static class DeviceErrors
{
    public static readonly Error DeviceNotFound =
       new("Device.Device Not Found.", "the Device With this Id Is Not Found ", StatusCodes.Status404NotFound);

}
