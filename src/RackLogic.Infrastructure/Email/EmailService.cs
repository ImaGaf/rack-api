using System.Net;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RackLogic.Infrastructure.Email;
public class EmailService : IEmailSender
{
  private readonly IConfiguration _configuration;
  public EmailService(IConfiguration configuration)
  {
    _configuration = configuration;
  }
  private SendGridClient GetClient()
  {
    var apiKey = _configuration.GetSection("SendGrid:ApiKey").Value;
    return new SendGridClient(apiKey);
  }

  public async Task<bool> SendAsync(string toEmail, string subject, string content)
  {
    try
    {
      var client = GetClient();

      var from = new EmailAddress(_configuration.GetSection("SendGrid:SenderEmail").Value);

      var to = new EmailAddress(toEmail);

      var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

      var response = await client.SendEmailAsync(msg);

      return response.StatusCode == HttpStatusCode.Accepted;
    }
    catch
    {
      throw;
    }
  }

  public async Task SendSWLSignageAsync(string file, string? message, string? subject, string? email)
  {
    try
    {
      var client = GetClient();

      var from = new EmailAddress(_configuration.GetSection("SendGrid:SenderEmail").Value);

      var to = new EmailAddress(email);

      var msg = MailHelper.CreateSingleEmail(from, to, "Send", "", message);

      msg.AddAttachment("file.pdf", file);

      var response = await client.SendEmailAsync(msg);
    }
    catch
    {
      throw;
    }
  }
}
