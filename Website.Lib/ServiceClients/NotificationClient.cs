using System.Net.Http.Json;

namespace Website.Lib.ServiceClients;


/// <summary>
/// Implements <see cref="INotification"/> for the Blazor WebAssembly project, posting messages to a controller on the server.
/// </summary>
public class NotificationClient : INotification
{
    private readonly HttpClient _httpClient;


    public NotificationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task Send(ContactMessage message)
    {
        await _httpClient.PostAsJsonAsync("api/Notification/PostContactMessage", message);
    }


    public async Task Send(RecruitmentEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("api/Notification/PostRecruitmentEnquiry", message);
    }


    public async Task Send(RealEstateInvestorEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("api/Notification/PostRealEstateInvestorEnquiry", message);
    }


    public async Task Send(VentureCapitalEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("api/Notification/PostVentureCapitalEnquiry", message);
    }
}
