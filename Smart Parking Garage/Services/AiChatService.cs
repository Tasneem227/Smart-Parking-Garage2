using System.Text;
using System.Text.Json;
using Smart_Parking_Garage.Contracts.Chatbot;

namespace Smart_Parking_Garage.Services;

public class AiChatService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AiChatService> _logger;

    public AiChatService(HttpClient httpClient,ILogger<AiChatService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string?> SendAsync(
        string userId,
        string message,
        float latitude,
        float longitude)
    {
        var request = new AiChatRequest
        {
            UserId = userId,
            Message = message,
            latitude = latitude,
            longitude = longitude
        };

        var json = JsonSerializer.Serialize(request);

        _logger.LogInformation(
            "Sending AI request: {Payload}",
            json);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "https://chatbot-two-gold-69.vercel.app/chat",
            content);

        var responseText = await response.Content.ReadAsStringAsync();

        _logger.LogInformation(
            "AI response status: {StatusCode}, Response: {Response}",
            (int)response.StatusCode,
            responseText);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "AI request failed. Status: {StatusCode}, Response: {Response}",
                (int)response.StatusCode,
                responseText);

            throw new Exception(
                $"AI ERROR ({(int)response.StatusCode}): {responseText}");
        }

        var aiResponse = JsonSerializer.Deserialize<AiChatResponse>(
            responseText,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return aiResponse?.Response;
    }
}
