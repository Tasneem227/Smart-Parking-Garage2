using FluentValidation;

namespace Smart_Parking_Garage.Contracts.Booking;

public class BookingRequestValidator:AbstractValidator<BookingRequest>
{
    public BookingRequestValidator()
    {


        RuleFor(x => x.BookingStart)
            .NotEmpty().WithMessage("StartTime is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("StartTime must be in the future.");

        RuleFor(x => x.BookingEnd)
            .NotEmpty().WithMessage("EndTime is required.")
            .GreaterThan(x => x.BookingStart)
            .WithMessage("EndTime must be greater than StartTime.");
        RuleFor(x => x)
            .Must(x => x.BookingEnd >= x.BookingStart.AddMinutes(30))
            .WithMessage("Minimum booking duration is 30 minutes.");

        RuleFor(x => x.CarType)
            .NotEmpty()
            .WithMessage("Car Type is required.")
            .Must(carType => new[]
            {
                "compact",
                "motorcycle",
                "sedan",
                "suv",
                "truck",
                "van"
            }.Contains(carType, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Car Type must be one of: compact, motorcycle, sedan, suv, truck, van.");


    }
}
