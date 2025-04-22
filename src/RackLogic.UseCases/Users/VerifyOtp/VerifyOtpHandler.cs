using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.VerifyOtp;
public class VerifyOtpHandler(
  IRepositoryWithExpression<UserOtp> _userOtpRepository,
  UserManager<ApplicationUser> _userManager)
  : ICommandHandler<VerifyOtpCommand, Result<string?>>
{
  public async Task<Result<string?>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user is null) return Result.Invalid(new ValidationError("Email not found."));

    var userOtp = await _userOtpRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);

    if (userOtp is null) return Result.Invalid(new ValidationError("Otp invalid."));

    if (DateTime.UtcNow > userOtp.CreationTime.AddMinutes(3)) return Result.Invalid(new ValidationError("Otp expired."));

    var validToken = await _userManager.VerifyUserTokenAsync(
                                        user,
                                        "RESETPASSWORDTOKENPROVIDER",
                                        "ResetPassword",
                                        userOtp.ResetToken ?? string.Empty);
    var result = userOtp.Otp == request.Otp && validToken;

    if (result) return userOtp.ResetToken;

    return Result.Invalid(new ValidationError("Otp invalid."));
  }
}
