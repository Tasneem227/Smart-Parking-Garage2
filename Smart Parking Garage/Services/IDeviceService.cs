
using Smart_Parking_Garage.Contracts.Device;
using Smart_Parking_Garage.Contracts.IOT;
using Smart_Parking_Garage.Contracts.uploadedFile;

namespace Smart_Parking_Garage.Services;

public interface IDeviceService
{
    Task<Result<RegisterDeviceResponse>> RegisterDeviceAsync(RegisterDeviceRequest request, CancellationToken cancellationToken=default);
    Task<Result<DeviceResponse>> EnvironmentUpdateAsync(EnvironmentUpdateRequest request, CancellationToken cancellationToken=default);
    Task<Result<DeviceResponse>> SlotStatusUpdateAsync(SlotStatusUpdateRequest request, CancellationToken cancellationToken = default);
    Task<Result<DeviceResponse>> GateStatusUpdateAsync(GateStatusUpdateRequest request, CancellationToken cancellationToken=default);
    Task<Result> SendCommandAsync(DeviceCommandRequest request,CancellationToken cancellationToken = default);
    Task<Result> ProcessCommandAckAsync(DeviceCommandAckRequest request, CancellationToken cancellationToken = default);
    Task<Result> ExecuteCommandAsync(DeviceCommandRequest request, CancellationToken cancellationToken = default);
    Task<Result> RetryCommandAsync(DeviceCommand command, CancellationToken cancellationToken = default);
    Task<Result> OpenEntryGateAsync(string userId ,CancellationToken cancellationToken = default);
    Task<Result> OpenExitGateAsync(string userId ,CancellationToken cancellationToken = default);
    Task<Result> CaptureImageAsync(CancellationToken cancellationToken = default);
    Task<Result<FullGarageUploadResponse>> UploadAsync(FullGarageUploadImageRequest uploadImageRequest, CancellationToken cancellationToken = default);
    Task<Result> GasAlertAsync(GasAlertRequest gasAlertRequest, CancellationToken cancellationToken = default);
}

