using Smart_Parking_Garage.Contracts.Chatbot;
using System.Text.Json;

namespace Smart_Parking_Garage.Services;

public class AiChatService
{
    private readonly HttpClient _httpClient;

    public AiChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> SendAsync(
        string userId,
        string message,
        double latitude,
        double longitude)
    {
        var request = new AiChatRequest
        {
            UserId = userId,
            Message = message,
            Latitude = latitude,
            Longitude = longitude
        };

        var response = await _httpClient.PostAsJsonAsync(
            "https://polanaeem.pythonanywhere.com/chat",
            request
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception("AI ERROR: " + error);
        }

        var aiResponse =
            await response.Content.ReadFromJsonAsync<AiChatResponse>();

        return aiResponse?.Response;
    }
}
