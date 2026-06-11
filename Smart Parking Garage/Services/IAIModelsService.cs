using Smart_Parking_Garage.Contracts.uploadedFile;
using System.Threading;

namespace Smart_Parking_Garage.Services;

public interface IAIModelsService
{
    Task<Result<VehicleAiResponse>> ClassifyVehicleAsync(UploadedImageRequest image,string userid,CancellationToken cancellationToken=default);
}
