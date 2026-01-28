using FluentValidation;
using Smart_Parking_Garage.Contracts.ParkingSlot;

public class ParkingSlotRequestValidator : AbstractValidator<ParkingSlotRequest>
{
    public ParkingSlotRequestValidator()
    {
       
        RuleFor(x => x.SlotNumber)
            .NotEmpty()
            .WithMessage("Slot number is required.")
            .MaximumLength(10)
            .WithMessage("Slot number cannot exceed 10 characters.");

        
        RuleFor(x => x.SlotType)
            .NotEmpty()
            .WithMessage("Slot type is required.")
            .Must(type => type == "Normal" || type == "EV" || type == "Disabled")
            .WithMessage("Slot type must be one of: Normal, EV, Disabled.");

        
        RuleFor(x => x.IsOccupied)
            .NotNull()
            .WithMessage("Occupancy status is required.");

        
        RuleFor(x => x.LastUpdated)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Last updated time cannot be in the future.");

       
        RuleFor(x => x.SensorId)
            .GreaterThan(0)
            .When(x => x.SensorId.HasValue)
            .WithMessage("Sensor ID must be greater than 0.");
    }
}
