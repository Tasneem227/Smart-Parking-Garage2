using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Abstractions;
using Smart_Parking_Garage.Contracts.Device;
using Smart_Parking_Garage.Contracts.IOT;
using Smart_Parking_Garage.Contracts.uploadedFile;
using System.Security.Claims;
using System.Threading;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DeviceController(IDeviceService deviceService , IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IDeviceService _DeviceService = deviceService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDeviceRequest registerDeviceRequest, CancellationToken cancellationToken)
    {
        var result =await _DeviceService.RegisterDeviceAsync(registerDeviceRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("slots/update")]
    public async Task<IActionResult> SLotStatusUpdate([FromBody] SlotStatusUpdateRequest slotStatusUpdateRequest, CancellationToken cancellationToken)
    {
        var result = await _DeviceService.SlotStatusUpdateAsync(slotStatusUpdateRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    [HttpPost("gate/status")]
    public async Task<IActionResult> GateStatusUpdate([FromBody] GateStatusUpdateRequest gateStatusUpdateRequest, CancellationToken cancellationToken)
    {
        var result = await _DeviceService.GateStatusUpdateAsync(gateStatusUpdateRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("environment")]
    public async Task<IActionResult> EnvironmentUpdate([FromBody] EnvironmentUpdateRequest environmentUpdateRequest, CancellationToken cancellationToken)
    {
        var result = await _DeviceService.EnvironmentUpdateAsync(environmentUpdateRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("camera/upload")]
    public async Task<IActionResult> Upload([FromForm] FullGarageUploadImageRequest request, CancellationToken cancellationToken)
    {
        var result = await _DeviceService.UploadAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("alerts")]
    public async Task<IActionResult> GasAlert(GasAlertRequest request, CancellationToken cancellationToken)
    {
        var result = await _DeviceService.GasAlertAsync(request, cancellationToken);

        return result.IsSuccess ? Ok("Alert Sent Successfully") : result.ToProblem();
    }

    [HttpPost("open-entry")]
    public async Task<IActionResult> OpenEntryGate(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = await _bookingService.GetCurrentBookingForGateAsync(userId, cancellationToken);
        if (!booking.IsSuccess)
            return Problem(title: booking.Error.Code, detail: booking.Error.Description, statusCode: booking.Error.StatusCode);

        var result = await _DeviceService.OpenEntryGateAsync(userId, cancellationToken);
        return (result.IsSuccess) ? Ok("Entry gate opened successfully.") : Problem(title: result.Error.Code, detail: result.Error.Description, statusCode: result.Error.StatusCode);
        
    }

        [HttpPost("open-exit")]

    public async Task<IActionResult> OpenExitGate(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = await _bookingService.GetCurrentBookingForExitGateAsync(userId, cancellationToken);
        if (!booking.IsSuccess)
            return Problem(title: booking.Error.Code, detail: booking.Error.Description, statusCode: booking.Error.StatusCode);

        var result = await _DeviceService.OpenExitGateAsync(userId ,cancellationToken);
        return (result.IsSuccess) ? Ok("Exit gate opened successfully."): Problem(title: result.Error.Code, detail: result.Error.Description, statusCode: result.Error.StatusCode);

    }

    [HttpPost("capture-image")]
    public async Task<IActionResult> CaptureImage(CancellationToken cancellationToken)
    {
        await _DeviceService.CaptureImageAsync(cancellationToken);

        return Ok("Capture image command sent.");
    }

    [HttpPost("commands/ack")]
    public async Task<IActionResult> CommandAck([FromBody] DeviceCommandAckRequest request,CancellationToken cancellationToken)
    {
        await _DeviceService.ProcessCommandAckAsync(request,cancellationToken);

        return Ok("Command acknowledgement received ");
    }

   

    
}
