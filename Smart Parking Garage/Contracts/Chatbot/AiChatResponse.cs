using System.Text.Json.Serialization;

namespace Smart_Parking_Garage.Contracts.Chatbot;

public class AiChatResponse
{
    [JsonPropertyName("response")]
    public string? Response { get; set; }
}
