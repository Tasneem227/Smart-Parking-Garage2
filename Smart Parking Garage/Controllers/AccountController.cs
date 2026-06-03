using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Contracts.User;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _UserService = userService;

    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
       
        var result = await _UserService.GetProfileAsync(User.GetUserId()!);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateProfileRequest request)
    {
        var result = await _UserService.UpdateProfileAsync(User.GetUserId()!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _UserService.ChangePasswordAsync(User.GetUserId()!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
