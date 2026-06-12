namespace Smart_Parking_Garage.Entities;

public class AlertLog
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = default!;
    public string Type { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
