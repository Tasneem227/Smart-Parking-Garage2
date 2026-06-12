using FluentValidation;

namespace Smart_Parking_Garage.Contracts.IOT;

public class DeviceCommandAckRequestValidator
    : AbstractValidator<DeviceCommandAckRequest>
{
    public DeviceCommandAckRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty();

        RuleFor(x => x.CommandId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty()
             .Must(status =>
             {
                 var value = status.ToLower();

                 return value == "done"
                     || value == "failed";
             })
            .WithMessage("Status must be done or failed.");
    }

}