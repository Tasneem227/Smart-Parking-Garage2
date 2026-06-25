using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Constants;
using Smart_Parking_Garage.Contracts.Device;
using Smart_Parking_Garage.Contracts.IOT;
using Smart_Parking_Garage.Contracts.uploadedFile;
using Smart_Parking_Garage.Entities;
using Smart_Parking_Garage.Errors;
using System;

namespace Smart_Parking_Garage.Services;

public class DeviceService(IWebHostEnvironment webHostEnvironment
                            ,ApplicationDbContext context
                            ,INotificationService notificationService
                            ,ILogger<DeviceService> logger
                            , HttpClient httpClient , IBookingService bookingService ) :IDeviceService


{

    private readonly ApplicationDbContext _Context = context;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<DeviceService> _logger = logger;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IBookingService _bookingService = bookingService;
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/Uploads/Images";

    public async Task<Result<RegisterDeviceResponse>> RegisterDeviceAsync(RegisterDeviceRequest request, CancellationToken cancellationToken)
    {
        var gId = int.Parse(request.GarageId);
        bool garageExists = _Context.Garages.Any(x => x.GarageId == gId);
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
        return Result.Success(new RegisterDeviceResponse(request.DeviceId, gId, "Device registered successfully"));
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
        {
            return Result.Failure<DeviceResponse>(
                ParkingSlotErrors.SlotNotFound);
        }

        ParkingSlot.IsOccupied =request.Status.Equals("occupied", StringComparison.OrdinalIgnoreCase);

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
        var Gate = await _Context.Gates
                                .FirstOrDefaultAsync(
                                    x => x.GarageId == garageId &&
                                         x.GateType.ToLower() == request.Gate.ToLower(),
                                cancellationToken);
        if (Gate is null)
        {
            return Result.Failure<DeviceResponse>(GateErrors.GateNotFound);
        }
        Gate.Status = request.Status;
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(new DeviceResponse("Gate Status Updated Successfully"));
    }

    public async Task<Result> SendCommandAsync(DeviceCommandRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "https://smelting-remedial-unselect.ngrok-free.dev/device/commands",request , cancellationToken);

        
        Console.WriteLine(response);

        response.EnsureSuccessStatusCode();
        return Result.Success();
    }

    public async Task<Result> ExecuteCommandAsync(DeviceCommandRequest request,CancellationToken cancellationToken = default)
    {
        var command = new DeviceCommand
        {
            CommandId = request.CommandId,
            CommandType = request.Type,
            Status = "pending",
            RetryCount = 0,
            LastSentAt = DateTimeOffset.UtcNow
        };

        _Context.DeviceCommands.Add(command);

        await _Context.SaveChangesAsync(cancellationToken);

        await SendCommandAsync(request, cancellationToken);
        return Result.Success();
    }

    public async Task <Result> OpenEntryGateAsync(string userId, CancellationToken cancellationToken = default)
    {
        var bookingResult = await _bookingService.GetCurrentBookingForGateAsync(userId,cancellationToken);

        if (!bookingResult.IsSuccess)
            return Result.Failure(bookingResult.Error);

        var booking = bookingResult.Value;

        if (booking.LastEntryGateOpenedAt.HasValue &&
            booking.LastEntryGateOpenedAt.Value.AddMinutes(1) > DateTime.UtcNow)
        {
            return Result.Failure(DeviceErrors.EntryGateCooldown);
        }

        var open =  await ExecuteCommandAsync(
            new DeviceCommandRequest
            {
                CommandId = GenerateCommandId(),
                Type = DeviceCommands.OpenEntryGateType
            },cancellationToken);

        booking.LastEntryGateOpenedAt = DateTime.UtcNow;
        await _Context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> OpenExitGateAsync(string userId ,CancellationToken cancellationToken = default)
    {

        var bookingResult = await _bookingService.GetCurrentBookingForExitGateAsync( userId,cancellationToken);

        if (!bookingResult.IsSuccess)
            return Result.Failure(bookingResult.Error);

        var booking = bookingResult.Value;

        if (booking.LastExitGateOpenedAt.HasValue &&
            booking.LastExitGateOpenedAt.Value.AddMinutes(5) > DateTime.UtcNow)
        {
            return Result.Failure(DeviceErrors.ExitGateCooldown);
        }

        var exit = await ExecuteCommandAsync(
            new DeviceCommandRequest
            {
                CommandId = GenerateCommandId(),
                Type = DeviceCommands.OpenExitGateType
            },
            cancellationToken);

        booking.LastExitGateOpenedAt = DateTime.UtcNow;
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> CaptureImageAsync(CancellationToken cancellationToken = default)
    {
       var capture = await ExecuteCommandAsync(
            new DeviceCommandRequest
            {
                CommandId = GenerateCommandId(),
                Type = DeviceCommands.CaptureImage
            },
            cancellationToken);
        return Result.Success();
    }
   
    public async Task<Result> ProcessCommandAckAsync( DeviceCommandAckRequest request,CancellationToken cancellationToken = default)
    {
        var command = await _Context.DeviceCommands.FirstOrDefaultAsync( c => c.CommandId == request.CommandId,cancellationToken);

        if (command is null)
            throw new Exception("Command not found.");

        command.Status = request.Status.ToLower();

        command.Error = request.Error;

        command.DeviceId = request.DeviceId;

        command.TimeStamp = request.TimeStamp;

        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }


    public async Task<Result> RetryCommandAsync( DeviceCommand command,CancellationToken cancellationToken = default)
    {
        await SendCommandAsync(
            new DeviceCommandRequest
            {
                CommandId = command.CommandId,
                Type = command.CommandType
            },cancellationToken);

        command.RetryCount++;

        command.Status = "pending";

        command.LastSentAt = DateTimeOffset.UtcNow;

        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private static string GenerateCommandId()
    {
        return Guid.NewGuid().ToString("N");
    }



   
  
    public async Task<Result<FullGarageUploadResponse>> UploadAsync(FullGarageUploadImageRequest  uploadImageRequest , CancellationToken cancellationToken = default)
    {
        var ExistedDevice = await _Context.Devices.FirstOrDefaultAsync(x => x.DeviceId.Equals(uploadImageRequest.DeviceId), cancellationToken);
        if (ExistedDevice is null)
        {
            return Result.Failure<FullGarageUploadResponse>(DeviceErrors.DeviceNotFound);
        }

        var extension = Path.GetExtension(uploadImageRequest.File.FileName);

        var randomfilename = $"{Guid.NewGuid()}{extension}";

        var uploadedFile = new UploadedImage
        {
            ImageName = uploadImageRequest.File.FileName,
            ContentType = uploadImageRequest.File.ContentType,
            StoredImageName = randomfilename,
            ImageExtension = Path.GetExtension(uploadImageRequest.File.FileName),
            ImageType = uploadImageRequest.ImageType,

        };
        _logger.LogWarning(
    "Received Image => CommandId: {CommandId}, FileName: {FileName}, Time: {Time}",
    uploadImageRequest.CommandId,
    uploadImageRequest.File.FileName,
    DateTime.UtcNow);
        var path = Path.Combine(_imagesPath, randomfilename);
        var imageUrl =$"https://smartparkinggaragesystem.runasp.net/Uploads/Images/{randomfilename}";

        using var stream = File.Create(path);
        await uploadImageRequest.File.CopyToAsync(stream, cancellationToken);
        _logger.LogInformation("WebRootPath: {Path}", webHostEnvironment.WebRootPath);
        _logger.LogInformation("ImagesPath: {Path}", _imagesPath);

        await _Context.AddAsync(uploadedFile, cancellationToken);
        await _Context.SaveChangesAsync(cancellationToken);

        return Result.Success(new FullGarageUploadResponse( uploadedFile.Id,uploadImageRequest.CommandId) );
    }



    public async Task<Result> GasAlertAsync(GasAlertRequest  gasAlertRequest, CancellationToken cancellationToken = default)
    {
        var ExistedDevice = await _Context.Devices.FirstOrDefaultAsync(x => x.DeviceId.Equals(gasAlertRequest.DeviceId), cancellationToken);
        if (ExistedDevice is null)
        {
            return Result.Failure<DeviceResponse>(DeviceErrors.DeviceNotFound);
        }
        //Send Notification
        var garage = await _Context.Garages
           .FirstOrDefaultAsync(x => x.GarageId == ExistedDevice.GarageId, cancellationToken);

        if (garage is not null)
        {
            await _notificationService.SendAsync(
                "2e748d01-bff2-4147-9848-09e5cd5a7198",
                "Gas Alert",
                $"Gas leak detected in garage {garage.Name}. Immediate inspection is required.",
                "GasAlert"
            );
        }
        AlertLog alertLog = gasAlertRequest.Adapt<AlertLog>();
        await _Context.AlertLogs.AddAsync(alertLog, cancellationToken);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}