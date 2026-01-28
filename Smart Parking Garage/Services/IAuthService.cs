using Microsoft.AspNetCore.Identity.Data;
using Smart_Parking_Garage.Contracts.Authentication;


namespace Smart_Parking_Garage.Services;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string token, string RefreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(registerRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default);

}
