using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.VerifyOtp;
public class VerifyOtpHandler(
  IRepositoryWithExpression<UserOtp> _userOtpRepository,
  UserManager<ApplicationUser> _userManager)
  : ICommandHandler<VerifyOtpCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user is null) return Result.Invalid(new ValidationError("Email not found."));

    var userOtp = await _userOtpRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);

    if (userOtp is null) return false;

    var validToken = await _userManager.VerifyUserTokenAsync(
                                        user,
                                        _userManager.Options.Tokens.PasswordResetTokenProvider,
                                        UserManager<IdentityUser>.ResetPasswordTokenPurpose,
                                        userOtp.ResetToken ?? string.Empty);
    var result = userOtp.Otp == request.Otp && validToken;

    //if (result)
    //  await _userOtpRepository.DeleteAsync(userOtp);

    return result;
  }
}
