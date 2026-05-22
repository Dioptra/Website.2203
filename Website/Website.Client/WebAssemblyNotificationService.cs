using System.Net.Http.Json;
using Website.Client;

namespace Website.Shell.Client;

public sealed class WebAssemblyNotificationService(HttpClient httpClient) : INotification
{
    public async Task Send(ContactMessage message)
    {
        EnsureSuccess(await httpClient.PostAsJsonAsync("api/Notification/PostContactMessage", message).ConfigureAwait(false));
    }

    public async Task Send(RecruitmentEnquiry message)
    {
        EnsureSuccess(await httpClient.PostAsJsonAsync("api/Notification/PostRecruitmentEnquiry", message).ConfigureAwait(false));
    }

    public async Task Send(RealEstateInvestorEnquiry message)
    {
        EnsureSuccess(await httpClient.PostAsJsonAsync("api/Notification/PostRealEstateInvestorEnquiry", message).ConfigureAwait(false));
    }

    public async Task Send(VentureCapitalEnquiry message)
    {
        EnsureSuccess(await httpClient.PostAsJsonAsync("api/Notification/PostVentureCapitalEnquiry", message).ConfigureAwait(false));
    }

    private static void EnsureSuccess(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
    }
}