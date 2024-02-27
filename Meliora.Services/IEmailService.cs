namespace Meliora.Services;

public interface IEmailService
{
    Task SendEmailAsync(string from, string subject, string body);
}