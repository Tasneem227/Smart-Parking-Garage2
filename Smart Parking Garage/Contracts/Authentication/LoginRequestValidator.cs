using FluentValidation;

namespace Smart_Parking_Garage.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequestUser>
{
    public LoginRequestValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();

    }
}