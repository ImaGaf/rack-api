using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.ConfirmPassword;
public class ConfirmPasswordHandler(
    UserManager<ApplicationUser> _userManager,
    IRepositoryWithExpression<UserOtp> _userOtpRepository)
    : ICommandHandler<ConfirmPasswordCommand, Result<string>>
{
  public async Task<Result<string>> Handle(ConfirmPasswordCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null) return Result.Invalid(new ValidationError("Email not found."));

    // Check old password valid
    var passwordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);
    if (!passwordValid) return Result.Invalid(new ValidationError("Old password is incorrect."));

    // Create OTP and DeleteToken
    var otp = OTPGenerator.GenerateOTP(6);
    var deleteToken = await _userManager.GenerateUserTokenAsync(user, "DELETEPASSWORDTOKENPROVIDER", "DeleteUser");

    // Check if a record with the same UserId exists in UserOtp
    var existingOtp = await _userOtpRepository.FirstOrDefaultAsync(u => u.UserId == user.Id, cancellationToken);
    if (existingOtp != null)
    {
      await _userOtpRepository.DeleteAsync(existingOtp, cancellationToken); 
    }

    await _userOtpRepository.AddAsync(new UserOtp
    {
      UserId = user.Id,
      Otp = otp,
      DeleteToken = deleteToken,
      CreationTime = DateTime.Now,
    }, cancellationToken);

    return Result.Success(deleteToken);
  }
}
