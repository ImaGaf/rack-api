using System.Net.Mime;
using Azure;
using Azure.Communication.Email;
using RackLogic.Core.Interfaces;

namespace RackLogic.Infrastructure.Email;
public class ACSEmailService(EmailClient emailClient, string senderAddress)
  : IEmailSender
{
  public async Task<bool> SendAsync(string toEmail, string subject, string content)
  {
    var emailMessage = new EmailMessage(
        senderAddress: senderAddress,
        content: new EmailContent(subject)
        {
          PlainText = content,
          Html = content,
        },
        recipients: new EmailRecipients(new List<EmailAddress>
        {
          new EmailAddress(toEmail),
        }));


    var emailSendOperation = await emailClient.SendAsync(
        WaitUntil.Completed,
        emailMessage);

    return emailSendOperation.HasCompleted;
  }

  public async Task SendSWLSignageAsync(string file, string? subject, string? message, string? email)
  {
    var emailMessage = new EmailMessage(
      senderAddress: senderAddress,
      content: new EmailContent(subject)
      {
        PlainText = message,
        Html = message,
      },
      recipients: new EmailRecipients(new List<EmailAddress>
      {
        new EmailAddress(email),
      }));

    emailMessage.Attachments.Add(
      new EmailAttachment(
        "file.pdf",
        MediaTypeNames.Application.Pdf,
        new BinaryData(Convert.FromBase64String(file))
      )
    );

    await emailClient.SendAsync(
        WaitUntil.Started,
        emailMessage);
  }
}
