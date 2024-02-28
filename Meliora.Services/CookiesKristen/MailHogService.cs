using System.Net.Mail;

namespace Meliora.Services.CookiesKristen;

public class MailHogService : IMailHoglService
{
    public async Task SendEmailAsync(string from, string subject, string body)
    {
        var fromAddress = new MailAddress(from);
        var toAddress = new MailAddress("kristen@cookies.com", "Kristen");

        var smtp = new SmtpClient
        {
            Host = "mailhog",
            Port = 1025,
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };
        using var message = new MailMessage(fromAddress, toAddress);
        message.Subject = subject;
        message.Body = body;
        await smtp.SendMailAsync(message);
    }

}