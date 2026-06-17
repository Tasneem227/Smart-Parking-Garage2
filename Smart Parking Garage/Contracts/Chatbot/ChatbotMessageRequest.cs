namespace Smart_Parking_Garage.Contracts.Chatbot;

public class ChatbotMessageRequest
{
    public string message { get; set; } = string.Empty;
    public float latitude { get; set; }
    public float longitude { get; set; }
}
