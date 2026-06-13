using Microsoft.AspNetCore.Hosting;

namespace Smart_Parking_Garage.Services;

public class FileService(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context):IFileService
{
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/Uploads/Images";
    private readonly ApplicationDbContext _context = context;

    public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.UploadedImages.FindAsync(id);

        if (file is null)
            return ([], string.Empty, string.Empty);

        var path = Path.Combine(_imagesPath, file.StoredImageName);

        MemoryStream memoryStream = new();
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        return (memoryStream.ToArray(), file.ContentType, file.ImageName);
    }
}
