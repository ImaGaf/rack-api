namespace RackLogic.Core.Interfaces;

public interface IEmailSender
{
  public Task<bool> SendAsync(string toEmail, string subject, string content);
  Task SendSWLSignageAsync(string file, string? subject, string? message, string? email);
}
