using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Delete;
public class DeleteUserHandler(
  UserManager<ApplicationUser> _userManager,
  IUserTokenRepository _userTokenRepository,
  IRepositoryWithExpression<UserOtp> _userOtpRepository,
  IEmailSender _emailSender,
  IOptions<ClientOption> _clientOption) 
  : ICommandHandler<DeleteUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Result.Invalid(new ValidationError("User not found."));
    var userId = Guid.Parse(userIdClaim.Value);

    var user = await _userManager.FindByIdAsync(userId.ToString());
    if (user == null) return Result.Invalid(new ValidationError("User not found."));

    var validToken = await _userManager.VerifyUserTokenAsync(user, "DELETEPASSWORDTOKENPROVIDER", "DeleteUser", request.DeleteAccountToken);
    if (!validToken) return Result.Invalid(new ValidationError("Invalid delete token."));

    //Delete valid delete token after use
    var existingOtp = await _userOtpRepository.FirstOrDefaultAsync(u => u.UserId == user.Id, cancellationToken);
    if (existingOtp != null)
    {
      await _userOtpRepository.DeleteAsync(existingOtp, cancellationToken);
    }
    // Update DeleteDate
    user.DeleteDate = DateTime.UtcNow;
    await _userManager.UpdateAsync(user);

    // Delete record in AspNetUserTokens table with UserId = UserId delete
    await _userTokenRepository.DeleteTokensByUserIdAsync(user.Id, cancellationToken);

    var contentEmail = BuildDeleteAccountNotificationContent(_clientOption.Value.LogoMarketWithUrl, user.UserName!);
    return await _emailSender.SendAsync(user.Email!, EmailConstants.DeleteAccountUser, contentEmail);

  }

  private string BuildDeleteAccountNotificationContent(string logo, string userDeleteName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "DeleteAccountNotification.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    emailContent = emailContent.Replace("{logo}", logo);
    emailContent = emailContent.Replace("{userDeleteName}", userDeleteName);
    emailContent = emailContent.Replace("{currentYear}", DateTime.UtcNow.Year.ToString());

    return emailContent;
  }
}
