namespace Meliora.Services.CoockiesKristen;

public interface IEmailService
{
    Task SendEmailAsync(string from, string subject, string body);
}