
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
    Task SendCommandAsync(DeviceCommandRequest request,CancellationToken cancellationToken = default);
    Task ProcessCommandAckAsync(DeviceCommandAckRequest request, CancellationToken cancellationToken = default);
    Task ExecuteCommandAsync(DeviceCommandRequest request, CancellationToken cancellationToken = default);
    Task RetryCommandAsync(DeviceCommand command, CancellationToken cancellationToken = default);
    Task OpenEntryGateAsync(CancellationToken cancellationToken = default);
    Task OpenExitGateAsync(CancellationToken cancellationToken = default);
    Task CaptureImageAsync(CancellationToken cancellationToken = default);
    Task<Result<FullGarageUploadResponse>> UploadAsync(FullGarageUploadImageRequest uploadImageRequest, CancellationToken cancellationToken = default);
    Task<Result> GasAlertAsync(GasAlertRequest gasAlertRequest, CancellationToken cancellationToken = default);



}

