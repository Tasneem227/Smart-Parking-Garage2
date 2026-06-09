namespace Smart_Parking_Garage.Contracts.Device;

public class RegisterDeviceRequestValidator : AbstractValidator<RegisterDeviceRequest>
{
    public RegisterDeviceRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GarageId)
            .GreaterThan(0);

        RuleFor(x => x.SlotsCount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.TimeStamp)
            .NotEmpty();
        RuleFor(x => x)
    .Must(x =>
        x.HasCamera ||
        x.HasEnvSensors ||
        x.HasEntryGate ||
        x.HasExitGate)
    .WithMessage("At least one device capability must be enabled.");
    }
}

    
    
