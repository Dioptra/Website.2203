using System.Net.Http.Json;
using Website.Client;

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
        NotifyError(await _httpClient.PostAsJsonAsync("api/Notification/PostContactMessage", message).ConfigureAwait(false));
    }


    public async Task Send(RecruitmentEnquiry message)
    {
        NotifyError(await _httpClient.PostAsJsonAsync("api/Notification/PostRecruitmentEnquiry", message).ConfigureAwait(false));
    }


    public async Task Send(RealEstateInvestorEnquiry message)
    {
        NotifyError(await _httpClient.PostAsJsonAsync("api/Notification/PostRealEstateInvestorEnquiry", message).ConfigureAwait(false));
    }


    public async Task Send(VentureCapitalEnquiry message)
    {
        NotifyError(await _httpClient.PostAsJsonAsync("api/Notification/PostVentureCapitalEnquiry", message).ConfigureAwait(false));
    }


    private void NotifyError(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
    }
}
