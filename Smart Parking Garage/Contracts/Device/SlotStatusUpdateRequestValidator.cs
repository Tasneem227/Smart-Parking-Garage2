namespace Smart_Parking_Garage.Contracts.Device;

public class SlotStatusUpdateRequestValidator:AbstractValidator<SlotStatusUpdateRequest>
{
    public SlotStatusUpdateRequestValidator()
    {
        RuleFor(x => x.slotId)
             .NotEmpty()
             .ExclusiveBetween(0,9)
             .WithMessage("SlotId must be between 1 and 8.");

        RuleFor(x => x.IsOccupied)
            .NotNull();

        RuleFor(x => x.Timestamp)
            .NotEmpty();
    }
}
