using System.Net.Http.Json;
using Website.Lib;

namespace Website.WebAssembly;


/// <summary>
/// Implements <see cref="INotification"/> for the Blazor WebAssembly project, posting messages to a controller on the server.
/// </summary>
public class WebAssemblyNotificationService : INotification
{
    private readonly HttpClient _httpClient;


    public WebAssemblyNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task Send(ContactMessage message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostContactMessage", message);
    }


    public async Task Send(RecruitmentEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostRecruitmentEnquiry", message);
    }


    public async Task Send(RealEstateInvestorEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostRealEstateInvestorEnquiry", message);
    }


    public async Task Send(VentureCapitalEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostVentureCapitalEnquiry", message);
    }
}
