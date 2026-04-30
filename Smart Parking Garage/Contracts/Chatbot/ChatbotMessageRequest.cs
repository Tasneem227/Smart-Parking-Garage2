namespace Smart_Parking_Garage.Contracts.Chatbot;

public class ChatbotMessageRequest
{
    public string Message { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
