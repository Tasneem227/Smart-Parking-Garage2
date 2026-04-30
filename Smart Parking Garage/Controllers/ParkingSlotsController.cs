using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Smart_Parking_Garage.Authentication.Filters;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;



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
        var allSlots = await _parkingSlotService.GetAllSlotsAsync(cancellationToken);

        var response = allSlots.Adapt<IEnumerable<ParkingSlotResponse>>();
        return Ok(response) ;
    }


    [HasPermission(Permissions.GetParkingSlotsById)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSlotById(int id, CancellationToken cancellationToken)
    {
       var Slot = await _parkingSlotService.GetSlotByIdAsync(id, cancellationToken);
        if (Slot is null)
            return NotFound();

        var response = Slot.Adapt<ParkingSlotResponse>();
        return Ok(response);
    }

    [HasPermission(Permissions.AddParkingSlots)]
    [HttpPost("")]
    public async Task<IActionResult> CreateSlot([FromBody] ParkingSlotRequest request, CancellationToken cancellationToken)
    {
        var newSlot = await _parkingSlotService.CreateSlotAsync(request.Adapt<ParkingSlot>(), cancellationToken);
        var result = newSlot.Adapt<ParkingSlotResponse>();
        return CreatedAtAction(nameof(GetSlotById), new { id = result.ParkingSlotId }, result);
       
    }


    [HasPermission(Permissions.UpdateParkingSlots)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateParkingSlotRequest request, CancellationToken cancellationToken)
    {
        var IsUpdated = await _parkingSlotService.UpdateSlotAsync(id , request.Adapt<ParkingSlot>(), cancellationToken);
        if (!IsUpdated)
            return NotFound();

        return NoContent();
    }

    [HasPermission(Permissions.DeleteParkingSlots)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSlot(int id, CancellationToken cancellationToken)
    {
        var IsDeleted = await _parkingSlotService.DeleteSlotAsync(id, cancellationToken);
        if (!IsDeleted)
            return NotFound();

        return NoContent();
    }

    [HasPermission(Permissions.UpdateParkingSlots)]
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableSlots(CancellationToken cancellationToken)
    {
        var AvailableSlots = await _parkingSlotService.GetAvailableSlotsAsync(cancellationToken);

        var response = AvailableSlots.Adapt<IEnumerable<ParkingSlotResponse>>();
        return Ok(response);

    }

    [HasPermission(Permissions.UpdateParkingSlots)]
    [HttpPut("{id}/occupancy")]
    public async Task<IActionResult> UpdateOccupancy(int id, CancellationToken cancellationToken)
    {
        var IsUpdated = await _parkingSlotService.ToggleOccupancyAsync(id, cancellationToken);
        if (!IsUpdated)
            return NotFound();

        return NoContent();
    }
}
