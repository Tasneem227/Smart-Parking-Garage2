using FluentValidation;

namespace Smart_Parking_Garage.Contracts.Gate;

public class GateRequestValidator : AbstractValidator<GateRequest>
{
    public GateRequestValidator()
    {
        RuleFor(x => x.GateType)
            .NotEmpty().WithMessage("GateType is required.")
            .Must(t => t == "EntryGate" || t == "ExitGate"|| t == "entrygate" || t == "exitgate")
            .WithMessage("GateType must be either 'EntryGate' or 'ExitGate'.");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => new[] { "Open", "Closed", "Active", "Inactive", "Fault" , "open", "closed", "active", "inactive", "fault" }.Contains(s))
            .WithMessage("Status must be one of: Open, Closed, Active, Inactive, Fault.");
    }
}

