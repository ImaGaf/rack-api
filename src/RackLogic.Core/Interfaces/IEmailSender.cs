namespace RackLogic.Core.Interfaces;

public interface IEmailSender
{
  public Task<bool> SendAsync(string toEmail, string subject, string content);
}
