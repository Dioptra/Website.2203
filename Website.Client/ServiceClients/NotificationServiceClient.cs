using System.Net.Http.Json;

namespace Website.Client.ServiceClients;

public class NotificationServiceClient : INotificationServiceClient
{
    private readonly HttpClient _httpClient;


    public NotificationServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task SendNotification(ContactMessage message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostContactMessage", message);
    }

    public async Task SendNotification(RecruitmentEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostRecruitmentEnquiry", message);
    }

    public async Task SendNotification(RealEstateInvestorEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostRealEstateInvestorEnquiry", message);
    }

    public async Task SendNotification(VentureCapitalEnquiry message)
    {
        await _httpClient.PostAsJsonAsync("Notification/PostVentureCapitalEnquiry", message);
    }
}
