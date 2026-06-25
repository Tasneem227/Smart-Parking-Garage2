using static System.Net.Mime.MediaTypeNames;

namespace Smart_Parking_Garage.Contracts.uploadedFile;

public record FullGarageUploadResponse
(Guid imageId,
    string commandId,
    ParkingAiResponse SlotsStatusAnalysis);
