namespace Smart_Parking_Garage.Contracts.Device;

public sealed class GateStatusUpdateRequestValidator
    : AbstractValidator<GateStatusUpdateRequest>
{
    public GateStatusUpdateRequestValidator()
    {
     
        RuleFor(x => x.Gate)
     .Must(x =>
         string.Equals(x, "entrygate", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(x, "exitgate", StringComparison.OrdinalIgnoreCase))
           .WithMessage("Gate must be either 'EntryGate' or 'ExitGate'.");

        RuleFor(x => x.Status)
            .Must(x =>
                string.Equals(x, "open", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x, "closed", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Status must be either 'open' or 'closed'.");

        RuleFor(x => x.Timestamp)
            .NotEmpty()
             .WithMessage("Timestamp is required.");
    }
}