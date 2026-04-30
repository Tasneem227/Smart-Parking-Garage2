using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.User;
using Smart_Parking_Garage.Errors;

namespace Smart_Parking_Garage.Services;

public class UserService(UserManager<ApplicationUser> userManager
                         ,ApplicationDbContext context
                         , IRoleService roleService):IUserService
{
    private readonly UserManager<ApplicationUser> _UserManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IRoleService _roleService = roleService;

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
    await (from u in _context.Users
           join ur in _context.UserRoles
           on u.Id equals ur.UserId
           join r in _context.Roles
           on ur.RoleId equals r.Id into roles
           where !roles.Any(x => x.Name == DefaultRoles.Admin)
           select new
           {
               u.Id,
               u.FirstName,
               u.LastName,
               u.UserName,
               u.Email,
               u.PhoneNumber,
               u.IsDisabled,
               Roles = roles.Select(x => x.Name!).ToList()
           }
            )
            .GroupBy(u => new { u.Id, u.FirstName, u.LastName,u.UserName, u.Email,u.PhoneNumber, u.IsDisabled })
            .Select(u => new UserResponse
            (
                u.Key.Id,
                u.Key.FirstName,
                u.Key.LastName,
                u.Key.UserName,
                u.Key.Email,
                u.Key.PhoneNumber,
                u.Key.IsDisabled,
                u.SelectMany(x => x.Roles)
            ))
           .ToListAsync(cancellationToken);


    public async Task<Result<UserResponse>> GetAsync (string userId)
    {
        if (await _UserManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _UserManager.GetRolesAsync(user);

        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Success(response);
    }



    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _UserManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
        var UserNameIsExists = await _UserManager.Users.AnyAsync(x => x.UserName == request.UserName, cancellationToken);

        if (emailIsExists)
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        if (UserNameIsExists)
            return Result.Failure<UserResponse>(UserErrors.DuplicatedUserName);

        var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();

        var result = await _UserManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _UserManager.AddToRolesAsync(user, request.Roles);

            var response = (user, request.Roles).Adapt<UserResponse>();

            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> UpdateAsync(
    string id,
    UpdateUserRequest request,
    CancellationToken cancellationToken = default)
    {
        // 1. Check if user exists
        var user = await _UserManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // 2. Validate Email (only if provided)
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _UserManager.Users
                .AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken);

            if (emailExists)
                return Result.Failure(UserErrors.DuplicatedEmail);
        }

        // 3. Validate Username (only if provided)
        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            var userNameExists = await _UserManager.Users
                .AnyAsync(x => x.UserName == request.UserName && x.Id != id, cancellationToken);

            if (userNameExists)
                return Result.Failure(UserErrors.DuplicatedUserName);
        }

        // 4. Track old values (optional but useful)
        var oldEmail = user.Email;
        var oldUserName = user.UserName;

        // 5. Apply changes safely (NULLs ignored automatically)
        request.Adapt(user);

        // 6. Check if anything actually changed
        var isChanged =
            oldEmail != user.Email ||
            oldUserName != user.UserName ||
            request.FirstName is not null ||
            request.LastName is not null ||
            request.PhoneNumber is not null;

        // 7. Update user only if changed
        if (isChanged)
        {
            var result = await _UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(
                    new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }
        }

        // 8. Update roles ONLY if provided
        if (request.Roles is not null)
        {
            var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return Result.Failure(UserErrors.InvalidRoles);

            var currentRoles = await _UserManager.GetRolesAsync(user);

            if (!currentRoles.SequenceEqual(request.Roles))
            {
                await _UserManager.RemoveFromRolesAsync(user, currentRoles);
                await _UserManager.AddToRolesAsync(user, request.Roles);
            }
        }

        return Result.Success();
    }

    public async Task<Result> ToggleStatus(string id)
    {
        if (await _UserManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user.IsDisabled = !user.IsDisabled;

        var result = await _UserManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> Unlock(string id)
    {
        if (await _UserManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await _UserManager.SetLockoutEndDateAsync(user, null);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }



    public async Task<Result<UserProfileResponse>> GetProfileAsync (string UserId)
    {
        var user = await _UserManager.Users
        .Where(x => x.Id == UserId)
        .ProjectToType<UserProfileResponse>()
        .SingleOrDefaultAsync();

        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(user);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var userExists = await _UserManager.Users
       .AnyAsync(x => x.Id == userId);

        if (!userExists)
            return Result.Failure(UserErrors.UserNotFound);

        // 2. Check username duplication
        var isUserNameExists = await _UserManager.Users
            .AnyAsync(x => x.UserName == request.UserName && x.Id != userId);

        if (isUserNameExists)
            return Result.Failure(UserErrors.DuplicatedUserName);


        await _UserManager.Users
         .Where(x => x.Id == userId)
         .ExecuteUpdateAsync(setters => setters
             .SetProperty(x => x.FirstName, x => request.FirstName ?? x.FirstName)
             .SetProperty(x => x.LastName, x => request.LastName ?? x.LastName)
             .SetProperty(x => x.UserName, x => request.UserName ?? x.UserName)
             .SetProperty(x => x.PhoneNumber, x => request.PhoneNumber ?? x.PhoneNumber)
     );

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        if(await _UserManager.FindByIdAsync(userId) is not { } user) 
            return Result.Failure(UserErrors.UserNotFound);
        var result = await _UserManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

}
