using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.CookiesKristen.MailHogDependencies;
using System.Text.Json;

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

            foreach (var message in messages.items)
            {
                
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
}
