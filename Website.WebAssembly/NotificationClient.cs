using Website.Client;
using System.Net.Http.Json;

namespace Website.WebAssembly;


public class NotificationClient : INotification
{
    private readonly HttpClient _httpClient;


    public NotificationClient(HttpClient httpClient)
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
