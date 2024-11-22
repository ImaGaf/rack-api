using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.ResetPassword;
public class ResetPasswordHandler(
  IRepositoryWithExpression<UserOtp> _userOtpRepository,
  UserManager<ApplicationUser> _userManager)
  : ICommandHandler<ResetPasswordCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
  {

    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user is null)
    {
      // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
      // returned a 400 for an invalid code given a valid user email.
      return Result.Invalid(new ValidationError(_userManager.ErrorDescriber.InvalidToken().Description));
    }

    IdentityResult result;
    try
    {
      result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.NewPassword);
    }
    catch (FormatException)
    {
      result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
    }

    if (!result.Succeeded)
    {
      return Result.Invalid(new ValidationError(result.Errors?.FirstOrDefault()?.Description));
    }
    await _userOtpRepository.DeleteAsync(x => x.UserId == user.Id, true, cancellationToken);
    return true;
  }
}
