using System.Net.Mail;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EventAssos.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EventAssos.Core.Services;

public class EmailService(IConfiguration config) : IEmailService
{
  public async Task EnvoyerMailAsync(string email, string subject, string message)
  {
    await using var client = new ServiceBusClient(config["ServiceBusConnectionString"]);
    ServiceBusSender sender = client.CreateSender("email-queue");
    
    var emailPayload = new {To = email, Subject = subject, Message = message};
    string jsonPayload = JsonSerializer.Serialize(emailPayload);
    
    ServiceBusMessage busMessage = new ServiceBusMessage(jsonPayload);
    await sender.SendMessageAsync(busMessage);
  }
}