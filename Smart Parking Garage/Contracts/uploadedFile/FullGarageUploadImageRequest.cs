namespace Smart_Parking_Garage.Contracts.uploadedFile;


public class FullGarageUploadImageRequest
{
    public  string DeviceId { get; set; }
    public string CommandId { get; set; }
    public string ImageType { get; set; }
    public IFormFile File { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}