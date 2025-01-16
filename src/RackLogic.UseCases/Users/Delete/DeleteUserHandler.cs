using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Delete;
public class DeleteUserHandler(
  UserManager<ApplicationUser> _userManager,
  IUserTokenRepository _userTokenRepository,
  IEmailSender _emailSender,
  IConfiguration _configuration) 
  : ICommandHandler<DeleteUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByIdAsync(request.UserId.ToString());
    if (user == null) return Result.Invalid(new ValidationError("User not found."));

    var validToken = await _userManager.VerifyUserTokenAsync(user, "DELETEPASSWORDTOKENPROVIDER", "DeleteUser", request.DeleteAccountToken);
    if (!validToken) return Result.Invalid(new ValidationError("Invalid delete token."));

    // Update DeleteDate
    user.DeleteDate = DateTime.UtcNow;
    await _userManager.UpdateAsync(user);

    // Delete record in AspNetUserTokens table with UserId = UserId delete
    await _userTokenRepository.DeleteTokensByUserIdAsync(user.Id, cancellationToken);

    // Send email notification
    var baseUrl = _configuration.GetSection("Dashboard:BaseUrl").Value;
    var logo = $"{baseUrl}{_configuration.GetSection("Dashboard:LogoMarket").Value}";

    var contentEmail = BuildDeleteAccountNotificationContent(logo, user.UserName!);
    return await _emailSender.SendAsync(user.Email!, EmailConstants.DeleteAccountUser, contentEmail);

  }

  private string BuildDeleteAccountNotificationContent(string logo, string userDeleteName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "DeleteAccountNotification.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    emailContent = emailContent.Replace("{logo}", logo);
    emailContent = emailContent.Replace("{userDeleteName}", userDeleteName);
    emailContent = emailContent.Replace("{currentYear}", DateTime.Now.Year.ToString());

    return emailContent;
  }
}
