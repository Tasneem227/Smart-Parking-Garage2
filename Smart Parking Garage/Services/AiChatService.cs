using Smart_Parking_Garage.Contracts.Chatbot;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

    public async Task<string?> SendAsync(ChatbotMessageRequest request,string token)
    {
       
        var json = JsonSerializer.Serialize(request);

                _logger.LogInformation(
                    "Sending AI request: {Payload} {token}",
                    json,token);

        var content = new StringContent( json, Encoding.UTF8,  "application/json");

        var httpRequest = new HttpRequestMessage(
         HttpMethod.Post,
         "https://chatbot-two-gold-69.vercel.app/chat");
        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        httpRequest.Content = content;

        var response = await _httpClient.SendAsync(httpRequest);
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
