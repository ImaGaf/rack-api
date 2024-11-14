using System.Text;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
      var userOtp = await _userOtpRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
      result = await _userManager.ResetPasswordAsync(user, userOtp?.ResetToken ?? string.Empty, request.NewPassword);
    }
    catch (FormatException)
    {
      result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
    }

    if (!result.Succeeded)
    {
      return Result.Error(new ErrorList(result.Errors?.Select(x => x.Description)));
    }

    return Result.Success();
  }
}
