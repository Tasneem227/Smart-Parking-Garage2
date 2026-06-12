using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Abstractions;
using Smart_Parking_Garage.Contracts.Device;
using Smart_Parking_Garage.Contracts.uploadedFile;
using Smart_Parking_Garage.Contracts.IOT;
using System.Security.Claims;
using System.Threading;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> Upload(FullGarageUploadImageRequest request, CancellationToken cancellationToken)
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

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var booking = await _bookingService.GetCurrentBookingForGateAsync(userId, cancellationToken);

        if (booking is null)
            return BadRequest("you can not open the gate , No valid booking found.");

        if (booking.BookingStart.AddMinutes(-5) > DateTime.UtcNow)
        {
            return BadRequest("You can open Entry gate only 5 minutes before booking start time.");
        }

        await _DeviceService.OpenEntryGateAsync(cancellationToken);

        return Ok( "Entry gate opened successfully.");
    }

    [HttpPost("open-exit")]

    public async Task<IActionResult> OpenExitGate(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var booking = await _bookingService.GetCurrentBookingForExitGateAsync(userId, cancellationToken);

        if (booking is null)
            return BadRequest("you can not open the gate , No valid booking found.");


        await _DeviceService.OpenExitGateAsync(cancellationToken);

        return Ok("Exit gate opened successfully.");
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
