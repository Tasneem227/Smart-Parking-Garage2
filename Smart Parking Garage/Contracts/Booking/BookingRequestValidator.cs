using FluentValidation;

namespace Smart_Parking_Garage.Contracts.Booking;

public class BookingRequestValidator:AbstractValidator<BookingRequest>
{
    public BookingRequestValidator()
    {
        RuleFor(x => x.SlotNumber)
            .NotEmpty();

        RuleFor(x => x.BookingStart)
            .NotEmpty().WithMessage("StartTime is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be in the future.");

        RuleFor(x => x.BookingEnd)
            .NotEmpty().WithMessage("EndTime is required.");

        RuleFor(x => x)
            .Must(x => x.BookingEnd > x.BookingStart)
            .WithMessage("EndTime must be greater than StartTime.");

        //RuleFor(x => x)
        //    .Must(x => (x.BookingEnd - x.BookingStart).TotalHours <= 24)
        //    .WithMessage("You cannot book a slot for more than 24 hours.");
    }
}
