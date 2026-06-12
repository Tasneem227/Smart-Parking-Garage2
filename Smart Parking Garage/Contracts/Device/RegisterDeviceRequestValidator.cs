namespace Smart_Parking_Garage.Contracts.Device;

public class RegisterDeviceRequestValidator : AbstractValidator<RegisterDeviceRequest>
{
    public RegisterDeviceRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GarageId)
            .NotEmpty()
            .Must(id => int.TryParse(id, out var value) && value > 0)
            .WithMessage("GarageId must be a number greater than 0.");

        RuleFor(x => x.SlotsCount)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Slots Count Should Not Be Less Than 1");


        RuleFor(x => x.timestamp)
            .NotEmpty()
            .WithMessage("Timestamp is required.");
        RuleFor(x => x)
    .Must(x =>
        x.HasCamera ||
        x.HasEnvSensors ||
        x.HasEntryGate ||
        x.HasExitGate)
    .WithMessage("At least one device capability must be enabled.");
    }
}

    
    
