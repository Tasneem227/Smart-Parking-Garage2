namespace Smart_Parking_Garage.Services;

public interface IFileService
{
    Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default);

}
