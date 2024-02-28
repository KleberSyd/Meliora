namespace Meliora.Services.CookiesKristen;

public interface IEmailService
{
    Task SendEmailAsync(string from, string subject, string body);
}