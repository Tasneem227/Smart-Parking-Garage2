namespace Smart_Parking_Garage.Contracts.Device;

public class EnvironmentUpdateValidator:AbstractValidator<EnvironmentUpdateRequest>
{
    public EnvironmentUpdateValidator()
    {

        RuleFor(x => x.Temperature)
             .InclusiveBetween(-50, 100)
             .When(x => x.Temperature.HasValue)
             .WithMessage("Temperature must be between -50°C and 100°C.");

        RuleFor(x => x.Humidity)
            .InclusiveBetween(0, 100)
            .When(x => x.Humidity.HasValue)
            .WithMessage("Humidity must be between 0% and 100%.");
    }
}
