namespace Smart_Parking_Garage.Contracts.Device;

public class SlotStatusUpdateRequestValidator:AbstractValidator<SlotStatusUpdateRequest>
{
    public SlotStatusUpdateRequestValidator()
    {
        RuleFor(x => x.slotId)
             .NotEmpty()
             .ExclusiveBetween(0,9)
             .WithMessage("SlotId must be between 1 and 8.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status =>
                status.Equals("occupied", StringComparison.OrdinalIgnoreCase) ||
                status.Equals("free", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Status must be either 'occupied' or 'free'.");

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .WithMessage("Timestamp is required.");
    }
}
