using FluentValidation;
using Smart_Parking_Garage.Abstractions.Consts;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Smart_Parking_Garage.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<registerRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x=>x.LastName)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone Number Is Required")
            .Length(11)
            .Matches(RegexPatterns.phone)
            .WithMessage("Phone number must be a valid Egyptian mobile number (11 digits starting with 01).");
    }
}