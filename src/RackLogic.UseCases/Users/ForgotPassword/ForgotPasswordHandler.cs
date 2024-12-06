using System.Text.Encodings.Web;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.ForgotPassword;
public class ForgotPasswordHandler(
  IConfiguration _configuration,
  IRepositoryWithExpression<UserOtp> _userOtpRepository,
  UserManager<ApplicationUser> _userManager)
 // IEmailSender _emailSender) 
  : ICommandHandler<ForgotPasswordCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user is not null)
    {
      if (user.LockedUntil.HasValue && user.LockedUntil.Value > DateTime.UtcNow) return Result.Forbidden("OTP is locked. Please try again later.");

      await _userOtpRepository.DeleteAsync(x => x.UserId == user.Id);
      var otp = OTPGenerator.GenerateOTP(6);
      var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
      await _userOtpRepository.AddAsync(new UserOtp
      {
        UserId = user.Id,
        Otp = otp,
        ResetToken = resetToken,
        CreationTime = DateTime.Now,
      });

      var baseUrl = _configuration.GetSection("Dashboard:BaseUrl").Value;
      var logo = $"{baseUrl}{_configuration.GetSection("Dashboard:LogoMarket").Value}";
      var forgotPasswordUrl = _configuration.GetSection("Dashboard:ForgotPassword").Value;
      var resetLink = $"{baseUrl}{forgotPasswordUrl}?userId={user.Id}&resetToken={UrlEncoder.Default.Encode(resetToken)}";
      var content = BuildEmailContent($"{user.FirstName} {user.LastName}", otp, resetLink, logo);
      //var result = await _emailSender.SendAsync(request.Email, EmailConstants.OTPSubject, content);
      var result = true;
      
      if (result)
      {
        await IncrementOtpCountAsync(user);
      }
      return result;
    }

    return Result.Invalid(new ValidationError("Email not found."));
  }

  private async Task IncrementOtpCountAsync(ApplicationUser user)
  {
    user.OtpSendCount++;
    if (user.OtpSendCount > 0 && user.OtpSendCount % 5 == 0)
    {
      user.LockedUntil = DateTime.UtcNow.AddMinutes(20);
    }
    await _userManager.UpdateAsync(user);
  }

  private string BuildEmailContent(string firstName, string otp,string resetLink , string logo)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "ForgotPasswordOtp.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    emailContent = emailContent.Replace("{first_name}", firstName);
    emailContent = emailContent.Replace("{otp}", otp);
    //emailContent = emailContent.Replace("{reset_link}", resetLink);
    emailContent = emailContent.Replace("{logo}", logo);
    emailContent = emailContent.Replace("{currentYear}", DateTime.Now.Year.ToString());

    return emailContent;
  }
}
