using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smart_Parking_Garage.Contracts.Authentication;
using Smart_Parking_Garage.Services;
using Smart_Parking_Garage.Settings;
using static Smart_Parking_Garage.Services.AuthService;


namespace SurveyBusket8.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IConfiguration configuration,
                            IOptions<JwtOptions> JwtOptions,
                            ILogger<AuthController> logger,
                            UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _AuthService = authService;
    private readonly IConfiguration _Configuration = configuration;
    private readonly ILogger<AuthController> _Logger = logger;
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly JwtOptions _JwtOptions = JwtOptions.Value;

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequestUser loginRequest,CancellationToken cancellationToken)
    {
        var authResult= await _AuthService.GetTokenAsync(loginRequest.Email, loginRequest.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();

    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _AuthService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _AuthService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] registerRequest registerRequest, CancellationToken cancellationToken)
    {
        var Result = await _AuthService.RegisterAsync(registerRequest, cancellationToken);

        return Result.IsFailure ? Result.ToProblem() : Ok();

    }

    [HttpGet("ConfirmEmail")]

    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string UserId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        var request = new ConfirmEmailRequest
        {
            UserId = UserId,
            code = code
        };
        var Result = await _AuthService.ConfirmEmailAsync(request);

        return Result.IsFailure ? Result.ToProblem() : Ok();

    }
    [HttpPost("ResendConfirmEmail")]
    public async Task<IActionResult> ResendConfirmEmailAsync([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var Result = await _AuthService.ResendConfirmEmailAsync(request);

        return Result.IsFailure ? Result.ToProblem() : Ok();

    }

    //[HttpPost("send-phone-confirmation")]
    //public async Task<IActionResult> SendPhoneConfirmation(
    //CancellationToken cancellationToken)
    //{
    //    var result = await _AuthService.SendPhoneConfirmationAsync(
    //        User.GetUserId());

    //    return result.IsSuccess
    //        ? Ok()
    //        : result.ToProblem();
    //}

    //[HttpPost("confirm-phone")]
    //public async Task<IActionResult> ConfirmPhone(
    //[FromBody] ConfirmPhoneRequest request,
    //CancellationToken cancellationToken)
    //{
    //    var result = await _AuthService.ConfirmPhoneAsync(
    //        User.GetUserId(),
    //        request.Code);

    //    return result.IsSuccess
    //        ? Ok()
    //        : result.ToProblem();
    //}

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] forgetPasswordRequest request )
    {
        var result = await _AuthService.SendResetPasswordCodeAsync(request.email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] resetPasswordRequest request)
    {
        var result = await _AuthService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    //[HttpGet("")]
    //public IActionResult Test()
    //{
    //    var _config = new
    //    {
    //        mykey = _JwtOptions.key,
    //        //connectionString = _Configuration["ConnectionStrings:DefaultConnections"],
    //        //Hello_java = _Configuration["Hello.java"],
    //        //ASPNETCORE_ENVIRONMENT = _Configuration["ASPNETCORE_ENVIRONMENT"]
    //    };
    //    return Ok(_config);
    //}


}
