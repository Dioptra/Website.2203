#if BLAZOR_WEBASSEMBLY // Remove controller from Blazor Server where it isn't needed and is therefore an unnecessary attack vector
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Website.Lib;

namespace Website.Server;


/// <summary>
/// Receives notifications from the Blazor WebAssembly client app. Applies messages received to <see cref="ServerNotificationService"/>
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private static readonly string _messagingWebhook = Environment.GetEnvironmentVariable("MESSAGING_WEBHOOK") ?? "https://nonexistent.nothing";

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }


    private async Task GenericSend(IMessage message)
    {
        try
        {
            var client = new HttpClient();

            var json = message.GetMessageCardJson(_serializerOptions);

            var response = await client.PostAsync(_messagingWebhook, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new NotSupportedException($"Received failed result {response.StatusCode} when posting events to Microsoft Teams.");
            }

            _logger.LogInformation($"Sent message to Teams using {_messagingWebhook}; received this response: {response.StatusCode}", message, response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact message to Teams", message);
        }
    }


    /// <summary>
    /// POST "contact me" messages.
    /// </summary>
    /// <param name="contactMessage"></param>
    /// <returns></returns>
    [HttpPost("PostContactMessage")]
    public async Task PostContactMessage([FromBody] ContactMessage contactMessage)
    {
        await GenericSend(contactMessage).ConfigureAwait(false);
    }


    /// <summary>
    /// POST client/real estate investor enquries.
    /// </summary>
    /// <param name="contactMessage"></param>
    /// <returns></returns>
    [HttpPost("PostRealEstateInvestorEnquiry")]
    public async Task PostRealEstateInvestorEnquiry([FromBody] RealEstateInvestorEnquiry realEstateInvestorEnquiry)
    {
        await GenericSend(realEstateInvestorEnquiry).ConfigureAwait(false);
    }


    /// <summary>
    /// POST recruitment enquries.
    /// </summary>
    /// <param name="contactMessage"></param>
    /// <returns></returns>
    [HttpPost("PostRecruitmentEnquiry")]
    public async Task PostRecruitmentEnquiry([FromBody] RecruitmentEnquiry recruitmentEnquiry)
    {
        await GenericSend(recruitmentEnquiry).ConfigureAwait(false);
    }


    /// <summary>
    /// POST VC investor enquries.
    /// </summary>
    /// <param name="contactMessage"></param>
    /// <returns></returns>
    [HttpPost("PostVentureCapitalEnquiry")]
    public async Task PostVentureCapitalEnquiry([FromBody] VentureCapitalEnquiry ventureCapitalEnquiry)
    {
        await GenericSend(ventureCapitalEnquiry).ConfigureAwait(false);
    }
}
#endif