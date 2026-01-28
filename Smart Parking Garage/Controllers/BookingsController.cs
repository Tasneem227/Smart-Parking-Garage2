using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Services;
using System.Security.Claims;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _BookingService = bookingService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddBookingAsync([FromBody]BookingRequest request,CancellationToken cancellationToken)
    {
        var response =await _BookingService.AddBooking(request,cancellationToken);
        if(response == null)
        {
            return BadRequest("Invalid Booking");
        }
        return Ok(response);
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAllBookingsAsync(CancellationToken cancellationToken)
    {
        var response = await _BookingService.GetAllAsync(cancellationToken);
        if (response == null)
        {
            return BadRequest("There are no bookings Available");
        }
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingByIdAsync([FromRoute]int id,CancellationToken cancellationToken)
    {
        var response = await _BookingService.GetByIdAsync(id,cancellationToken);
        if (response == null)
        {
            return BadRequest("There are no bookings Available");
        }
        return Ok(response);
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBookingByUserIdAsync([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var response = await _BookingService.GetByUserIdAsync(userId, cancellationToken);
        if (response == null)
        {
            return BadRequest("There are no bookings Available");
        }
        return Ok(response);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateBookingTimeAsync([FromRoute] int Id, [FromBody]updateBookingTimeRequest request ,CancellationToken cancellationToken)
    {
        var response = await _BookingService.UpdateBookingTimeAsync(Id, request,cancellationToken);
        if (response == null)
        {
            return BadRequest("There are no bookings With this Id");
        }
        return Ok(response);
    }
    [HttpPut("updateBookingStatus/{Id}")]
    public async Task<IActionResult> UpdateBookingStatusAsync([FromRoute] int Id, [FromBody] UpdateBookingStatusRequest status , CancellationToken cancellationToken)
    {
      var result= await _BookingService.UpdateBookingStatusAsync(Id, status, cancellationToken);
        return result == true ? Ok("Status updated successfully") : BadRequest("Invalid Booking Id");
            }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookingByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _BookingService.DeleteByIdAsync(id, cancellationToken);
        return NoContent();
    }

}
