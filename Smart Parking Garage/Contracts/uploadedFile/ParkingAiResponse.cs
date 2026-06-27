namespace Smart_Parking_Garage.Contracts.uploadedFile;

public class ParkingAiResponse
{
    public string ImageUrl { get; set; } = string.Empty;
    public List<ParkingSlotDto> Slots { get; set; } = new();
}

public class ParkingSlotDto
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}