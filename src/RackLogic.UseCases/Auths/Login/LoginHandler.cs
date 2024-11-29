using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Auths.Login;
public class LoginHandler(
  SignInManager<ApplicationUser> _signInManager,
  UserManager<ApplicationUser> _userManager,
  IUserService _userService) : ICommandHandler<LoginCommand, Result<LoginDTO>>
{
  public async Task<Result<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null)
      return Result.Invalid(new ValidationError("The email and/or password entered is incorrect. Please try again."));

    if (await _userManager.IsLockedOutAsync(user))
    {
      var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
      if (lockoutEnd.HasValue && lockoutEnd.Value == DateTimeOffset.MaxValue)
        return Result.Forbidden("Your account has been permanently locked due to repeated login failures.");

      return Result.Forbidden("Account is locked. Please try again later.");
    }
      
    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
    if (result.Succeeded)
    {
      var token = await _userService.GenerateUserTokenAsync(user);
      return token;
    }

    //await _userManager.AccessFailedAsync(user);
    var failedCount = await _userManager.GetAccessFailedCountAsync(user);

    if (failedCount == 10)
    {
      await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));
      return Result.Forbidden("Account is locked. Please try again later.");
    }

    if (failedCount >= 20)
    {
      await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
      return Result.Forbidden("Your account has been permanently locked due to repeated login failures.");
    }

    return Result.Invalid(new ValidationError("The email and/or password entered is incorrect. Please try again."));
  }
}
