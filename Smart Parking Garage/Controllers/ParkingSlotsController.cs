using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Smart_Parking_Garage.Abstractions;
using Smart_Parking_Garage.Authentication.Filters;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.Gate;
namespace Smart_Parking_Garage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingSlotsController(IParkingSlotService parkingSlotService) : ControllerBase
{
    private readonly IParkingSlotService _parkingSlotService = parkingSlotService;

    [HasPermission(Permissions.GetParkingSlots)]
    [HttpGet("AllSlots")]
    public async Task<IActionResult> GetAllSlots(CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.GetAllSlotsAsync(cancellationToken);
        return  Ok(result.Value) ;
    }

    [HasPermission(Permissions.GetParkingSlotsById)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSlotById(int id, CancellationToken cancellationToken)
    {
       var result = await _parkingSlotService.GetSlotByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HasPermission(Permissions.AddParkingSlots)]
    [HttpPost("")]
    public async Task<IActionResult> CreateSlot([FromBody] ParkingSlotRequest request, CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.CreateSlotAsync(request, cancellationToken);
        return (result.IsSuccess) ? CreatedAtAction(nameof(GetSlotById), new { id = result.Value.ParkingSlotId }, result.Value) :
                Problem(title: result.Error.Code, detail: result.Error.Description, statusCode: result.Error.StatusCode);
    }

    [HasPermission(Permissions.GetParkingSlots)]
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableSlots(CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.GetAvailableSlotsAsync(cancellationToken);
        return Ok(result.Value);
    }

    //[HasPermission(Permissions.DeleteParkingSlots)]
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteSlot(int id, CancellationToken cancellationToken)
    //{
    //    var result = await _parkingSlotService.DeleteSlotAsync(id, cancellationToken);
    //    return (result.IsSuccess) ? NoContent() : NotFound(result.Error);
    //}


    [HasPermission(Permissions.UpdateParkingSlots)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateParkingSlotRequest request, CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.UpdateSlotAsync(id, request, cancellationToken);
        return (!result.IsSuccess) ? Problem(title: result.Error.Code, detail: result.Error.Description, statusCode: result.Error.StatusCode) : NoContent();
    }

    [HasPermission(Permissions.UpdateParkingSlots)]
    [HttpPut("{id}/occupancy")]
    public async Task<IActionResult> UpdateOccupancy(int id, CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.ToggleOccupancyAsync(id, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : NoContent();
    }

    [HasPermission(Permissions.GetParkingSlots)]
    [HttpGet("garage/{garageId}")]
    public async Task<IActionResult> GetSlotsByGarageId( int garageId,CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService .GetSlotsByGarageIdAsync(garageId, cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : Ok(result.Value);
    }

    [HasPermission(Permissions.GetParkingSlots)]
    [HttpGet("garage/{garageId}/available")]
    public async Task<IActionResult> GetAvailableSlotsByGarageId( int garageId,CancellationToken cancellationToken)
    {
        var result = await _parkingSlotService.GetAvailableSlotsByGarageIdAsync(garageId,cancellationToken);
        return (!result.IsSuccess) ? NotFound(result.Error) : Ok(result.Value);
  
    }
}
