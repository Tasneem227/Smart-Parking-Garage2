using FluentValidation;
using Smart_Parking_Garage.Contracts.Payment;

namespace Smart_Parking_Garage.Validators;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0);

        RuleFor(x => x.CardNumber)
            .NotEmpty()
            .Length(16)
            .Matches(@"^\d+$")
            .WithMessage("Card number must contain only digits.");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
            .WithMessage("Expiry date must be in MM/YY format.");

        RuleFor(x => x.CVV)
            .NotEmpty()
            .Length(3)
            .Matches(@"^\d+$")
            .WithMessage("CVV must contain only digits.");
    }
}