namespace Smart_Parking_Garage.Entities;

public class UploadedImage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string ImageName { get; set; } = string.Empty;
    public string StoredImageName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string ImageExtension { get; set; }=string.Empty;
    public string ImageType {  get; set; } = string.Empty;

}
