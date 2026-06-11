namespace Smart_Parking_Garage.Contracts.Device;

public sealed class GateStatusUpdateRequestValidator
    : AbstractValidator<GateStatusUpdateRequest>
{
    public GateStatusUpdateRequestValidator()
    {
     
        RuleFor(x => x.Gate)
     .Must(x =>
         string.Equals(x, "entry", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(x, "exit", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.Status)
            .Must(x =>
                string.Equals(x, "open", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x, "closed", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.Timestamp)
            .NotEmpty();
    }
}