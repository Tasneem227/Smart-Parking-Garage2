
namespace Smart_Parking_Garage.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
       new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "this Email Is Duplicated", StatusCodes.Status409Conflict);
    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "this Email Not Confirmed", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode =
        new("User.InvalidCode", "The code is not valid", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedConfirmation =
        new("User.DuplicatedConfirmation", "The Confirmation on the email is duplicated", StatusCodes.Status400BadRequest);

}
