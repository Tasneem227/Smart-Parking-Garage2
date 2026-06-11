using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Device;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DeviceController(IDeviceService deviceService) : ControllerBase
{
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

}
