using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Authentication.Filters;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Garage;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GaragesController(IGarageService garageService) : ControllerBase
{
    private readonly IGarageService _garageService = garageService;


    // Returns all garages locations (Id, Latitude, Longitude)
    [HasPermission(Permissions.GetGarages)]
    [HttpGet("locations")]
    public async Task<IActionResult> GetAllGarageLocations(CancellationToken cancellationToken = default)
    {
        var garages = await _garageService.GetAllGarageLocationsAsync(cancellationToken);
        return Ok(garages);
    }


    [HasPermission(Permissions.GetGaragesStatus)]
    [HttpGet("slots-status/{garageId}")]
    public async Task<IActionResult> GetSlotsStatus(int garageId, CancellationToken cancellationToken = default)
    {
        var result = await _garageService.GetSlotsStatusByGarageIdAsync(garageId, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HasPermission(Permissions.GetGarages)]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _garageService.GetAllAsync());


    [HasPermission(Permissions.GetGarageById)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var garage = await _garageService.GetByIdAsync(id);
        return garage == null ? NotFound() : Ok(garage);
    }

    [HasPermission(Permissions.AddGarages)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GarageCreate dto)
    {
        await _garageService.CreateAsync(dto);
        return Ok();
    }

    [HasPermission(Permissions.UpdateGarages)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GarageCreate dto)
    {
        var updated = await _garageService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HasPermission(Permissions.DeleteGarages)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _garageService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    [HasPermission(Permissions.GetGarageGateByOwnerId)]
    [HttpGet("GarageOwner-Garage-Gates")]
    public async Task<IActionResult> GarageOwnerGaragesAndGates(string GarageOwnerId)
    {
        var result = await _garageService.GarageOwnerGaragesAndGates(GarageOwnerId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HasPermission(Permissions.GarageRevenue)]
    [HttpGet("revenue/{garageId}")]
    public async Task<IActionResult> GetGarageRevenue(
    int garageId,
    CancellationToken cancellationToken)
    {
        var result = await _garageService.GetGarageRevenueAsync(
            garageId,
            cancellationToken);

        return result.IsSuccess
            ? Ok(new
            {
                GarageId = garageId,
                TotalRevenue = result.Value
            })
            : result.ToProblem();
    }
}
