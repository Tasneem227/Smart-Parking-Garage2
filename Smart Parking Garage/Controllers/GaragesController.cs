using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.Garage;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GaragesController(IGarageService garageService) : ControllerBase
{
    private readonly IGarageService _garageService=garageService;

    
    // Returns all garages locations (Id, Latitude, Longitude)
    [HttpGet("locations")]
    public async Task<IActionResult> GetAllGarageLocations(CancellationToken cancellationToken=default)
    {

        var garages = await _garageService.GetAllGarageLocationsAsync(cancellationToken);
        return Ok(garages);
    }



    [HttpGet("slots-status/{garageId}")]
    public async Task<IActionResult> GetSlotsStatus(
    int garageId,
    CancellationToken cancellationToken = default)
    {
        var result = await _garageService
            .GetSlotsStatusByGarageIdAsync(garageId, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _garageService.GetAllAsync());


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var garage = await _garageService.GetByIdAsync(id);
        return garage == null ? NotFound() : Ok(garage);
    }

  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GarageCreate dto)
    {
        await _garageService.CreateAsync(dto);
        return Ok();
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GarageCreate dto)
    {
        var updated = await _garageService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _garageService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
