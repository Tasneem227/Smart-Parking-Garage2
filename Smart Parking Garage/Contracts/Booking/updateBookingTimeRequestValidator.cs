using FluentValidation;

namespace Smart_Parking_Garage.Contracts.Booking;

public class updateBookingTimeRequestValidator:AbstractValidator<updateBookingTimeRequest>
{
    public updateBookingTimeRequestValidator()
    {
     

        RuleFor(x => x.BookingStart)
            .NotEmpty().WithMessage("StartTime is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be in the future.");

        RuleFor(x => x.BookingEnd)
            .NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x)
            .Must(x => x.BookingEnd > x.BookingStart)
            .WithMessage("EndTime must be greater than StartTime.");
    }
}