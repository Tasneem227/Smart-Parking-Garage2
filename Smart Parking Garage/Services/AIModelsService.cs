using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Garage;
using Smart_Parking_Garage.Contracts.uploadedFile;
using Smart_Parking_Garage.Errors;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;

namespace Smart_Parking_Garage.Services;

public class AIModelsService(HttpClient httpClient 
                            ,IWebHostEnvironment webHostEnvironment
                            ,ApplicationDbContext context
                            ,UserManager<ApplicationUser> userManager 
                            ,ILogger<AIModelsService> logger) : IAIModelsService
{
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/Uploads/images";

    private readonly HttpClient _HttpClient = httpClient;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly ILogger<AIModelsService> _Logger = logger;

    public async Task<Result<VehicleAiResponse>> ClassifyVehicleAsync(UploadedImageRequest uploadedImageRequest,
                                                            string userid,CancellationToken cancellationToken)
    {
        var user = await _UserManager.FindByIdAsync(userid.ToString());

        if (user is null)
            return Result.Failure<VehicleAiResponse>(
                UserErrors.UserNotFound);


        if (uploadedImageRequest.Image == null || uploadedImageRequest.Image.Length == 0)
            return Result.Failure<VehicleAiResponse>(UploadedFileErrors.EmptyImageFile);

        using var formData = new MultipartFormDataContent();

        await using var stream = uploadedImageRequest.Image.OpenReadStream();

        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(uploadedImageRequest.Image.ContentType);

        formData.Add(
            fileContent,
            "file",
            uploadedImageRequest.Image.FileName);

        var response = await _HttpClient.PostAsync(
            "https://vehicle-classification-system-4eqj.vercel.app/api/classify",
            formData);

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Response: {responseBody}");

        var result = JsonSerializer.Deserialize<VehicleAiResponse>(responseBody);

        if (result is null)
        {
            throw new Exception("Failed to deserialize AI response.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception (
                $"Vehicle AI Error ({(int)response.StatusCode}): {responseBody}");
        }

        var uploadedFile = await SaveFile(uploadedImageRequest.Image, cancellationToken);
        CarType carType = new CarType
        {
            UserId = userid,
            carType=result.CarType
        };
        await _context.AddAsync(uploadedFile, cancellationToken);
        await _context.AddAsync(carType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(result)!;
    }
    private async Task<UploadedImage> SaveFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        var randomFileName = Path.GetRandomFileName();

        var uploadedFile = new UploadedImage
        {
            ImageName = file.FileName,
            ContentType = file.ContentType,
            StoredImageName = randomFileName,
            ImageExtension = Path.GetExtension(file.FileName),
            ImageType=ImageTypes.VehicleClassification
        };

        var path = Path.Combine(_imagesPath, randomFileName);

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return uploadedFile;
    }
    public async Task<ParkingAiResponse?> AnalyzeParkingImageAsync(
    UploadedGarageImageRequest uploadedImageRequest ,
    CancellationToken cancellationToken = default)
    {
        if (uploadedImageRequest.photo == null || uploadedImageRequest.photo.Length == 0)
            throw new Exception("Image file is empty.");

        using var formData = new MultipartFormDataContent();

        await using var stream = uploadedImageRequest.photo.OpenReadStream();

        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(uploadedImageRequest.photo.ContentType);

        formData.Add(
            fileContent,
            "photo",
            uploadedImageRequest.photo.FileName);

        var response = await _HttpClient.PostAsync(
            "https://parking-spot-occupancy.vercel.app/fixed-camera/analyze",
            formData,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Response: {responseBody}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Parking AI Error ({(int)response.StatusCode}): {responseBody}");
        }

        var result = JsonSerializer.Deserialize<ParkingAiResponse>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null)
            throw new Exception("Failed to deserialize AI response.");

        return result??new();
    }
}
