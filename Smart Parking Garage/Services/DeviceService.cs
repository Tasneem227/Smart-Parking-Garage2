using Smart_Parking_Garage.Contracts.Device;
using Smart_Parking_Garage.Errors;

namespace Smart_Parking_Garage.Services;

public class DeviceService(ApplicationDbContext context) :IDeviceService
{
    private readonly ApplicationDbContext _Context = context;

    public async Task<Result<RegisterDeviceResponse>> RegisterDeviceAsync(RegisterDeviceRequest request, CancellationToken cancellationToken)
    {
        bool garageExists = _Context.Garages.Any(x => x.GarageId == request.GarageId);
        if (!garageExists)
        {
            return Result.Failure<RegisterDeviceResponse>(GarageErrors.GarageNotFound);
        }
        var ExistedDevice =await _Context.Devices.FirstOrDefaultAsync(x => x.DeviceId.Equals(request.DeviceId),cancellationToken);
        if (ExistedDevice is null)
        {
            Device newDevice = request.Adapt<Device>();
            await _Context.AddAsync(newDevice, cancellationToken);
        }
        else
        {
            request.Adapt(ExistedDevice);
        }
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(new RegisterDeviceResponse(request.DeviceId, request.GarageId, "Device registered successfully"));
    }


    public async Task<Result<DeviceResponse>> EnvironmentUpdateAsync(EnvironmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var ExistedDevice = await _Context.Devices.AnyAsync(x => x.DeviceId.Equals(request.DeviceId), cancellationToken);
        if (!ExistedDevice)
        {
            return Result.Failure<DeviceResponse>(DeviceErrors.DeviceNotFound);
        }
        var environmentReading =await _Context.EnvironmentReadings.FirstOrDefaultAsync(x => x.DeviceId.Equals(request.DeviceId),cancellationToken);
        
        EnvironmentReading EnvironmentReading = request.Adapt<EnvironmentReading>();
        await _Context.AddAsync(EnvironmentReading, cancellationToken);
        
        
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(new DeviceResponse("Environment Readings Updated Successfully"));
    }



    public async Task<Result<DeviceResponse>> SlotStatusUpdateAsync(SlotStatusUpdateRequest request, CancellationToken cancellationToken)
    {
        var ExistedDevice = await _Context.Devices.FirstOrDefaultAsync(x => x.DeviceId.Equals(request.DeviceId), cancellationToken);
        if (ExistedDevice is null)
        {
            return Result.Failure<DeviceResponse>(DeviceErrors.DeviceNotFound);
        }
        var garageId = ExistedDevice.GarageId;
        var ParkingSlot=await _Context.ParkingSlots.FirstOrDefaultAsync(x=>x.GarageId==garageId&&x.SlotNumber==request.slotId.ToString(),cancellationToken);
        if (ParkingSlot is null)
            throw new Exception($"Slot {request.slotId} not found.");
        ParkingSlot.IsOccupied = (bool)request.IsOccupied;
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(new DeviceResponse("Slot Status Updated Successfully"));
    }

    public async Task<Result<DeviceResponse>> GateStatusUpdateAsync(GateStatusUpdateRequest request, CancellationToken cancellationToken)
    {
        var ExistedDevice = await _Context.Devices.FirstOrDefaultAsync(x => x.DeviceId.Equals(request.DeviceId), cancellationToken);
        if (ExistedDevice is null)
        {
            return Result.Failure<DeviceResponse>(DeviceErrors.DeviceNotFound);
        }
        var garageId = ExistedDevice.GarageId;
        var Gate = await _Context.Gates.FirstOrDefaultAsync(x => x.GarageId == garageId && x.GateType == request.Gate, cancellationToken);
        if (Gate is null)
            throw new Exception($"Gate {Gate.GateId} not found.");
        Gate.Status = request.Status;
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(new DeviceResponse("Gate Status Updated Successfully"));
    }



}