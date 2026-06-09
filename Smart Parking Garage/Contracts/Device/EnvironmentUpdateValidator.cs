namespace Smart_Parking_Garage.Contracts.Device;

public class EnvironmentUpdateValidator:AbstractValidator<EnvironmentUpdateRequest>
{
    public EnvironmentUpdateValidator()
    {

        RuleFor(x => x.Temperature)
            .InclusiveBetween(-50, 100)
            .When(x => x.Temperature.HasValue);

        RuleFor(x => x.Humidity)
            .InclusiveBetween(0, 100)
            .When(x => x.Humidity.HasValue);
    }
}
