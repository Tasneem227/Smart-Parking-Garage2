using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Abstractions;
using Smart_Parking_Garage.Contracts.Gate;
using Smart_Parking_Garage.Services;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]

[ApiController]
public class GatesController (IGateService gateService): ControllerBase
{
    private readonly IGateService _gateService = gateService;

    [HttpGet("AllGates")]
    [Authorize]
    public async Task<IActionResult> GetAllGates(CancellationToken cancellationToken)
    {
        var result = await _gateService.GetAllGatesAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGateById([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result = await _gateService.GetGateByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateGate([FromBody] GateRequest request, CancellationToken cancellationToken)
    {
        var result = await _gateService.CreateGateAsync(request, cancellationToken);
        return (result.IsSuccess) ? CreatedAtAction(nameof(GetGateById), new { id = result.Value.GateId },result.Value) :
                Problem(title: result.Error.Code, detail: result.Error.Description,statusCode: result.Error.StatusCode);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGate([FromRoute]int id, [FromBody] UpdateGateRequest request, CancellationToken cancellationToken)
    {
        var result = await _gateService.UpdateGateAsync(id, request, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGate([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _gateService.DeleteGateAsync(id, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : NoContent() ; 
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateGateStatus( [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _gateService.UpdateGateStatusAsync(id, cancellationToken);
        return (!result.IsSuccess) ? NotFound (result.Error) : NoContent() ; 
    }

    [HttpGet("garage/{garageId}")]
    public async Task<IActionResult> GetGarageGates(int garageId,CancellationToken cancellationToken)
    {
        var result = await _gateService.GetGatesByGarageIdAsync(garageId, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : Ok(result.Value);
    }

}
