using FluentValidation;
using Smart_Parking_Garage.Contracts.IOT;

namespace Smart_Parking_Garage.Contracts.Device;

public class DeviceCommandRequestValidator : AbstractValidator<DeviceCommandRequest>
{
    public DeviceCommandRequestValidator()
    {
        RuleFor(x => x.CommandId)
            .NotEmpty()
            .WithMessage("CommandId is required.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Command type is required.")
            .Must(type =>
                type == "OPEN_ENTRY_GATE" ||
                type == "OPEN_EXIT_GATE" ||
                type == "CAPTURE_IMAGE")
            .WithMessage("Command type must be OPEN_ENTRY_GATE or OPEN_EXIT_GATE or CAPTURE_IMAGE ");
    }
}

