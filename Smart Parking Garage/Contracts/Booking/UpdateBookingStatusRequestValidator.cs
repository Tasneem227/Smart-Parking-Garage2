namespace Smart_Parking_Garage.Contracts.Booking;

public class UpdateBookingStatusRequestValidator:AbstractValidator<UpdateBookingStatusRequest>
{
    public UpdateBookingStatusRequestValidator()
    {


        RuleFor(x => x.status)
            .NotEmpty()
            .WithMessage("Booking Status is required.")
            .Must(type => type == "Active" || type == "Completed" || type == "Pending" || type == "Cancelled" || type == "Expired"
            || type == "active" || type == "completed" || type == "pending" || type == "cancelled" || type == "expired")
            .WithMessage("Slot type must be one of: Active, Completed, Pending, Cancelled, expired.");

    }
}
