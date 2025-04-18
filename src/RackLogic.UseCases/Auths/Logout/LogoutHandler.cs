using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Auths.Logout;
public class LogoutHandler(
  UserManager<ApplicationUser> _userManager,
  IAuthService _userService,
  IUser _user)
  : ICommandHandler<LogoutCommand, Result>
{
  public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
  {
    if (_user.Id == null)
      return Result.Success();

    var username = await _userManager.FindByIdAsync(_user.Id?.ToString() ?? string.Empty);
    await _userService.UpdateSecurityStampByUserNameAsync(username?.UserName ?? string.Empty);

    return Result.Success();
  }
}
