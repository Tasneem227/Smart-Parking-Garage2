using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Gate;
using Smart_Parking_Garage.Services;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]

[ApiController]
public class GatesController (IGateService gateService): ControllerBase
{
    private readonly IGateService _gateService = gateService;

    [HttpGet("AllGates")]
  //  [Authorize]
    public async Task<IActionResult> GetAllGates(CancellationToken cancellationToken)
    {
        var allGates = await _gateService.GetAllGatesAsync(cancellationToken);

        var response = allGates.Adapt<IEnumerable<GateRequest>>();
        return Ok(response);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetGateById([FromRoute]int id, CancellationToken cancellationToken)
    {
        var Gate = await _gateService.GetGateByIdAsync(id, cancellationToken);
        if (Gate is null)
            return NotFound();

        var response = Gate.Adapt<GateResponse>();
        return Ok(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateGate([FromBody] GateRequest request, CancellationToken cancellationToken)
    {
        var newGate = await _gateService.CreateGateAsync(request.Adapt<Gate>(), cancellationToken);
        var result = newGate.Adapt<GateResponse>();
        return CreatedAtAction(nameof(GetGateById), new { id = result.GateId }, result);

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGate([FromRoute]int id, [FromBody] UpdateGateRequest request, CancellationToken cancellationToken)
    {
        var IsUpdated = await _gateService.UpdateGateAsync(id, request.Adapt<Gate>(), cancellationToken);
        if (!IsUpdated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGate([FromRoute] int id, CancellationToken cancellationToken)
    {
        var IsDeleted = await _gateService.DeleteGateAsync(id, cancellationToken);
        if (!IsDeleted)
            return NotFound();

        return NoContent();
    }


    /// //////////
 

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateGateStatus([FromRoute] int id, [FromBody] string status,CancellationToken cancellationToken)
    {

        var IsUpdated = await _gateService.UpdateGateStatusAsync(id,status ,cancellationToken);
        if (!IsUpdated)
            return NotFound();

        return NoContent();
    }
}
