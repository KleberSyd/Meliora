namespace Meliora.Services.CookiesKristen.Interfaces;

public interface IMailHoglService
{
    Task SendEmailAsync(string from, string subject, string body);
}