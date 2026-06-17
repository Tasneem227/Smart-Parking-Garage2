namespace Smart_Parking_Garage.Contracts.Chatbot;

public class ChatbotMessageRequestValidator:AbstractValidator<ChatbotMessageRequest>
{
    public ChatbotMessageRequestValidator()
    {
        RuleFor(x => x.message)
            .NotEmpty()
            .WithMessage("Message is required.")
            .MaximumLength(1000)
            .WithMessage("Message cannot exceed 1000 characters.");
        RuleFor(x => x.latitude)
            .NotEmpty()
            .WithMessage("latitude is required.");

        RuleFor(x => x.longitude)
            .NotEmpty()
            .WithMessage("longitude is required.");
    }
}
