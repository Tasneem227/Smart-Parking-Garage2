using Smart_Parking_Garage.Abstractions.Consts;

namespace Smart_Parking_Garage.Contracts.User;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email != null);


        RuleFor(x => x.FirstName)
            .Length(3, 100)
            .When(x => x.FirstName != null);


        RuleFor(x => x.LastName)
            .Length(3, 100)
            .When(x => x.LastName != null);


        RuleFor(x => x.UserName)
            .MinimumLength(3)
            .MaximumLength(20)
            .When(x => x.UserName != null); 

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.phone)
            .WithMessage("Invalid Egyptian phone number")
            .When(x => x.PhoneNumber != null); 

        RuleFor(x => x.Roles)
            .NotNull()
            .When(x => x.Roles != null);


        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot add duplicated role for the same user")
            .When(x => x.Roles != null);
    }
}