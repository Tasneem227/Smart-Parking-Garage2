namespace Smart_Parking_Garage.Contracts.SensorReading;
using System.Text.Json.Serialization;

public class ParkingDataJson
{
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("gas")]
    public int Gas { get; set; }

    [JsonPropertyName("total_slots")]
    public int TotalSlots { get; set; }

    [JsonPropertyName("occupied_slots")]
    public int OccupiedSlots { get; set; }

    [JsonPropertyName("slot1")]
    public bool Slot1 { get; set; }

    [JsonPropertyName("slot2")]
    public bool Slot2 { get; set; }

    [JsonPropertyName("slot3")]
    public bool Slot3 { get; set; }

    [JsonPropertyName("entry_gate")]
    public string EntryGate { get; set; }

    [JsonPropertyName("exit_gate")]
    public string ExitGate { get; set; }
}
