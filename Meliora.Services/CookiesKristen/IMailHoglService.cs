namespace Meliora.Services.CookiesKristen;

public interface IMailHoglService
{
    Task SendEmailAsync(string from, string subject, string body);
}