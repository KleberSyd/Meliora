using System.Runtime.InteropServices;
using Meliora.Domain.Enum;
using Meliora.Domain.Models.CookiesKristen;
using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.CookiesKristen.MailHogDependencies;
using System.Text.Json;
using System.Text.RegularExpressions;
using Meliora.Repository.Context;
using Meliora.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Services.CookiesKristen;

/**
 * In a real-world scenario, this service would be responsible to collect messages from email server using IMAP or POP3 protocol.
 * but the MailHog is a fake SMTP server for testing purposes, so we are using its API to fetch the messages.
 * and only I can retrieve the messages from the MailHog server using the API.
 */
public class MailNotificationService : IMailNotificationService
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Thread _thread;
    private readonly HttpClient _httpClient;

    public MailNotificationService()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _httpClient = new HttpClient();

        _thread = new Thread(async () =>
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(15));

                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    await FetchMailHogMessagesAsync();

                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            }
            catch (OperationCanceledException)
            {
                _cancellationTokenSource.Cancel();
            }

        });

        _thread.Start();
    }

    private async Task FetchMailHogMessagesAsync()
    {
        try
        {
            const string mailHogApiUrl = "http://mailhog:8025/api/v2/messages";
            var response = await _httpClient.GetAsync(mailHogApiUrl);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var messages = JsonSerializer.Deserialize<MailhogResponse>(responseBody);

            if (messages != null)
            {
                foreach (var message in messages.items)
                {
                    var quantityMatch = Regex.Match(message.Content.Body ?? string.Empty, @"Quantity: (\d+)");
                    var flavorsMatch = Regex.Match(message.Content.Body ?? string.Empty, @"Flavors: ([\w, ]+)");
                    var nameMatch = Regex.Match(message.Content.Body ?? string.Empty, @"Name: ([\w ]+)");


                    if (!quantityMatch.Success || !flavorsMatch.Success || !nameMatch.Success)
                    {
                        continue;
                    }
                    var quantity = int.Parse(quantityMatch.Groups[1].Value);
                    var flavors = flavorsMatch.Groups[1].Value.Split([", "], StringSplitOptions.None);
                    var name = nameMatch.Groups[1].Value;

                    var mixins = flavors.Select(flavor => new Mixin(name: flavor)).ToList();

                    var customer = new Customer(name, $"{message.From.Mailbox}@{message.From.Domain}");

                    // Workaround to use the DbContext in a non-HTTP context
                    // Blazor for some unkonwn reason is not working for Singleton and I could not pretty way to solve this for now
                    var optionsBuilder = new DbContextOptionsBuilder<CookieKristenDbContext>();
                    optionsBuilder.UseSqlServer("Server=db,1433;Database=CookieKristen;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True");

                    await using var context = new CookieKristenDbContext(optionsBuilder.Options);

                    await using var orderService = new OrderService(context);

                    var order = new Order(customer, quantity, mixins, OrderStatus.Pending);

                    try
                    {
                        await orderService.ProcessOrderAsync(order);
                        await DeleteMailHogMessageAsync(message.id);
                    }
                    catch (FailedToOrderCookieException e)
                    {
                        await new MailHogService().SendEmailAsync("kristen@cookies.com", e.Message, e.Message);
                    }
                }
            }

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    


    public void StopListening()
    {
        _cancellationTokenSource.Cancel();
        if (_thread.IsAlive)
        {
            _thread.Join();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
    public async Task DeleteMailHogMessageAsync(string messageId)
    {
        try
        {
            var mailHogApiUrl = $"http://mailhog:8025/api/v1/messages/{messageId}";
            var request = new HttpRequestMessage(HttpMethod.Delete, mailHogApiUrl);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message: {0}", e.Message);
        }
    }
}
