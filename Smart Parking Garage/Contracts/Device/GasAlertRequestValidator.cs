namespace Smart_Parking_Garage.Contracts.Device;

public class GasAlertRequestValidator:AbstractValidator<GasAlertRequest>
{
    public GasAlertRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("Device ID is required.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(x => x.Equals("gas_leak", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Alert Type is not supported.");

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .WithMessage("Timestamp is required.");
    }
}
