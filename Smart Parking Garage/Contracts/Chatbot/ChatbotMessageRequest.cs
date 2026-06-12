namespace Smart_Parking_Garage.Contracts.Chatbot;

public class ChatbotMessageRequest
{
    public string Message { get; set; } = string.Empty;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
