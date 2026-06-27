namespace Smart_Parking_Garage.Errors;

public static class DeviceErrors
{
    public static readonly Error DeviceNotFound =
       new("Device.Device Not Found.", "the Device With this Id Is Not Found ", StatusCodes.Status404NotFound);

    public static readonly Error EntryGateOpenTooEarly =
      new( "Gate.EntryGateOpenTooEarly", "You can open the entry gate only 1 minutes before the booking start time.",StatusCodes.Status400BadRequest);

    public static readonly Error EntryGateCooldown =
    new( "Device.EntryGateCooldown", "Entry gate can only be opened once every 1 minutes.",StatusCodes.Status400BadRequest);

    public static readonly Error ExitGateCooldown =
        new( "Device.ExitGateCooldown", "Exit gate can only be opened once every 1 minutes.",StatusCodes.Status400BadRequest);
    public static readonly Error CommandNotFound =
   new("Device.Command Not Found.", "Command Not Found ", StatusCodes.Status404NotFound);

}
