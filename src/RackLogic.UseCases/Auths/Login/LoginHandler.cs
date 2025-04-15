using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Auths.Login;
public class LoginHandler(
  SignInManager<ApplicationUser> _signInManager,
  UserManager<ApplicationUser> _userManager,
  IAuthService _userService)
  : ICommandHandler<LoginCommand, Result<LoginDTO>>
{
  public async Task<Result<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user is null)
      return Result.Invalid(new ValidationError("The email and/or password entered is incorrect. Please try again."));

    if (user.DeleteDate.HasValue)
      return Result.Forbidden("Your account is scheduled for deletion and cannot be accessed.");

    if (await _userManager.IsLockedOutAsync(user))
    {
      var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
      return Result.Forbidden(
          lockoutEnd == DateTimeOffset.MaxValue
              ? "Your account has been permanently locked due to repeated login failures."
              : "Account is locked. Please try again later."
      );
    }

    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
    if (result.Succeeded)
    {
      var token = await _userService.GenerateUserTokenAsync(user, request.WarehouseId);
      return token;
    }

    var failedCount = await _userManager.GetAccessFailedCountAsync(user);

    return failedCount switch
    {
      >= 20 => await LockAccountPermanently(user),
      10 => await LockAccountTemporarily(user),
      _ => Result.Invalid(new ValidationError("The email and/or password entered is incorrect. Please try again."))
    };
  }

  private async Task<Result<LoginDTO>> LockAccountTemporarily(ApplicationUser user)
  {
    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));
    return Result.Forbidden("Account is locked. Please try again later.");
  }

  private async Task<Result<LoginDTO>> LockAccountPermanently(ApplicationUser user)
  {
    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
    return Result.Forbidden("Your account has been permanently locked due to repeated login failures.");
  }
}
