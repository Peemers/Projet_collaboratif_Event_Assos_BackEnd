using System.Net.Mail;
using EventAssos.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EventAssos.Core.Services;

public class EmailService(IConfiguration config) : IEmailService
{
  public async Task EnvoyerMailAsync(string email, string subject, string message)
  {
    MimeMessage emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("Event'Assos Admin", config["SmtpSettings:SenderEmail"]!));
    emailMessage.To.Add(new MailboxAddress("",email));
    emailMessage.Subject = subject;
    emailMessage.Body = new TextPart("html") { Text = message };

    using SmtpClient client = new SmtpClient();

    await client.ConnectAsync(config["SmtpSettings:Server"], int.Parse(config["SmtpSettings:Port"]!), false);
    await client.AuthenticateAsync(config["SmtpSettings:Username"], config["SmtpSettings:Password"]);
    await client.SendAsync(emailMessage);
    await client.DisconnectAsync(true);


  }
}