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
    {
      return Result.Invalid(new ValidationError("Email not found."));
    }
    else
    {
      var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
      if (result.Succeeded)
      {
        var token = await _userService.GenerateUserTokenAsync(user);
        return token;
      }
      else
      {
        return Result.Invalid(new ValidationError(result.ToString()));
      }
    }
  }
}
