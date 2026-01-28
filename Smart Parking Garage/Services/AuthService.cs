using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Authentication;
using Smart_Parking_Garage.Contracts.Authentication;
using Smart_Parking_Garage.Entities;
using Smart_Parking_Garage.Errors;
using Smart_Parking_Garage.Helpers;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = Smart_Parking_Garage.Abstractions.Error;

namespace Smart_Parking_Garage.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
                        IJwtProvider jwtProvider,
                        SignInManager<ApplicationUser> signInManager
                        , ILogger<AuthService> logger,
                        IHttpContextAccessor httpContextAccessor
                        , IEmailSender emailSender) : IAuthService
{
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly IJwtProvider _JwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _SignInManager = signInManager;
    private readonly ILogger<AuthService> _Logger = logger;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _EmailSender = emailSender;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        //Check User
        var user = await _UserManager.FindByEmailAsync(email);
        if (user == null)
            return null;
        //Check Password
        var PasswordResult = await _SignInManager.PasswordSignInAsync(user, password, false, false);
        if (PasswordResult.Succeeded)
        {
            //Generate JWT Token
            var (token, expiresIn) = _JwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpirtion = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, ExpiresOn = refreshTokenExpirtion });
            await _UserManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpirtion);
            return response;
        }
        return null;
    }


    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId == null)
            return null;

        var user = await _UserManager.FindByIdAsync(userId);
        if (user == null) 
            return null;

        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == RefreshToken && rt.IsActive);
        if (existingRefreshToken == null)
            return null;

        existingRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newtoken, expiresIn) = _JwtProvider.GenerateToken(user);
        var newrefreshToken = GenerateRefreshToken();
        var refreshTokenExpirtion = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken { Token = newtoken, ExpiresOn = refreshTokenExpirtion });
        await _UserManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newtoken, expiresIn, newrefreshToken, refreshTokenExpirtion);
        return response;


    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId == null)
            return false;

        var user = await _UserManager.FindByIdAsync(userId);

        if (user == null)
            return false;

        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(
            rt => rt.Token == RefreshToken && rt.IsActive
            );

        if (existingRefreshToken == null)
            return false;

        //Revoke  Refresh Token
        existingRefreshToken.RevokedOn = DateTime.UtcNow;
        await _UserManager.UpdateAsync(user);

        return true;
    }


    //registration service
    public async Task<Result> RegisterAsync(registerRequest request, CancellationToken cancellationToken = default)
    {
        var EmailExists = await _UserManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (EmailExists)
        {
            return Result.Failure<AuthResponse?>(UserErrors.DuplicatedEmail);
        }
        var user = request.Adapt<ApplicationUser>();
        var result = await _UserManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _Logger.LogInformation("Confirmation Code :{code}", code);

            //send email
            SendConfirmationEmail(user, code);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure<AuthResponse?>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    //confirm email service
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        if (await _UserManager.FindByIdAsync(request.UserId) is not { } user)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }
        var code = request.code;
        if (user.EmailConfirmed)
        {
            return Result.Failure(UserErrors.DuplicatedConfirmation);
        }
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }
        //valid code confirm the email
        var result = await _UserManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
    {
        if (await _UserManager.FindByEmailAsync(request.Email) is not { } user)
        {
            return Result.Success();
        }

        if (user.EmailConfirmed)
        {
            return Result.Failure(UserErrors.DuplicatedConfirmation);
        }

        var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _Logger.LogInformation("Confirmation Code :{code}", code);

        //todo:send email
        SendConfirmationEmail(user, code);
        return Result.Success();
    }



    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _HttpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"https://localhost:7133/auth/ConfirmEmail?userId={user.Id}&code={code}" }
            }
        );

        await _EmailSender.SendEmailAsync(user.Email!, "✅ Smart Parking System : Email Confirmation", emailBody);
    }
    public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}