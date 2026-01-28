namespace Smart_Parking_Garage.Contracts.Authentication;

public class ResendEmailConfirmationRequestValidator:AbstractValidator<ResendEmailConfirmationRequest>
{
    public ResendEmailConfirmationRequestValidator()
    {

        RuleFor(x => x.email)
            .NotEmpty()
            .EmailAddress();

    }
}
