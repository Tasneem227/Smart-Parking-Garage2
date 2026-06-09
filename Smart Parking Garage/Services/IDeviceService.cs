using Smart_Parking_Garage.Contracts.Device;

namespace Smart_Parking_Garage.Services;

public interface IDeviceService
{
    Task<Result<RegisterDeviceResponse>> RegisterDeviceAsync(RegisterDeviceRequest request, CancellationToken cancellationToken=default);
    Task<Result<DeviceResponse>> EnvironmentUpdateAsync(EnvironmentUpdateRequest request, CancellationToken cancellationToken=default);
    Task<Result<DeviceResponse>> SlotStatusUpdateAsync(SlotStatusUpdateRequest request, CancellationToken cancellationToken = default);
    Task<Result<DeviceResponse>> GateStatusUpdateAsync(GateStatusUpdateRequest request, CancellationToken cancellationToken=default);



}
