using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Authentication;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
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
                        , IEmailSender emailSender
                        ,ApplicationDbContext context) : IAuthService
{
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly IJwtProvider _JwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _SignInManager = signInManager;
    private readonly ILogger<AuthService> _Logger = logger;
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _EmailSender = emailSender;
    private readonly ApplicationDbContext _Context = context;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (await _UserManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        var result = await _SignInManager.PasswordSignInAsync(user, password, false, true);

        if (result.Succeeded)
        {
            var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            var (token, expiresIn) = _JwtProvider.GenerateToken(user, userRoles, userPermissions);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _UserManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }

        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedUser
            : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);
    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _JwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _UserManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);

        var (newToken, expiresIn) = _JwtProvider.GenerateToken(user, userRoles, userPermissions);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _UserManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _JwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _UserManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _UserManager.UpdateAsync(user);

        return Result.Success();
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
            await _UserManager.AddToRoleAsync(user, DefaultRoles.Member);
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

    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
    {
        var userRoles = await _UserManager.GetRolesAsync(user);

        var userPermissions = await (from r in _Context.Roles
                                     join p in _Context.RoleClaims
                                     on r.Id equals p.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return (userRoles, userPermissions);
    }
    public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}