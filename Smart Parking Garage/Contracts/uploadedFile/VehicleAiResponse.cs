using System.Text.Json.Serialization;

namespace Smart_Parking_Garage.Contracts.uploadedFile;

public class VehicleAiResponse
{
    [JsonPropertyName("car_type")]
    public string CarType { get; set; } = string.Empty;
}
