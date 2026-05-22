using System.Text;
using System.Text.Json;
using Website.Client;

namespace Website;

public sealed class ServerNotificationService(ILogger<ServerNotificationService> logger) : INotification
{
    private static readonly string MessagingWebhook = Environment.GetEnvironmentVariable("MESSAGING_WEBHOOK") ?? "https://nonexistent.nothing";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    public Task Send(ContactMessage message) => GenericSend(message, logger);

    public Task Send(RecruitmentEnquiry message) => GenericSend(message, logger);

    public Task Send(RealEstateInvestorEnquiry message) => GenericSend(message, logger);

    public Task Send(VentureCapitalEnquiry message) => GenericSend(message, logger);

    private static async Task GenericSend(IMessage message, ILogger logger)
    {
        try
        {
            using var client = new HttpClient();
            var json = message.GetMessageCardJson(SerializerOptions);
            var response = await client.PostAsync(MessagingWebhook, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new NotSupportedException($"Received failed result {response.StatusCode} when posting events to Microsoft Teams.");
            }

            logger.LogInformation("Sent message to Teams using {MessagingWebhook}; received this response: {StatusCode}", MessagingWebhook, response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send contact message to Teams");
        }
    }
}