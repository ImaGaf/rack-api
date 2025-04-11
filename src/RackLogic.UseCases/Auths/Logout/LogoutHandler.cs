using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Auths.Logout;
public class LogoutHandler(
  IAuthService _userService)
  : ICommandHandler<LogoutCommand, Result>
{
  public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
  {
    if (request.user.Identity?.IsAuthenticated ?? false)
    {
      var username = request.user.Claims.First(x => x.Type == "UserName").Value;
      await _userService.UpdateSecurityStampByUserNameAsync(username);
    }

    return Result.Success();
  }
}
