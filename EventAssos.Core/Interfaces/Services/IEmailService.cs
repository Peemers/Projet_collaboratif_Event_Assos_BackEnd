namespace EventAssos.Core.Interfaces.Services;

public interface IEmailService
{
  Task EnvoyerMailAsync(string email, string subject,  string message);
}