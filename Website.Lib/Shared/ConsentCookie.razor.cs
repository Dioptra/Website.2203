using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;

namespace Website.Lib.Shared;
public partial class ConsentCookie : ComponentBase
{
    [Inject] private IHttpContextAccessor Http { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }


    private ITrackingConsentFeature ConsentFeature { get; set; }
    private bool ShowBanner { get; set; }
    private string CookieString { get; set; }


    protected override void OnInitialized()
    {
        ConsentFeature = Http.HttpContext.Features.Get<ITrackingConsentFeature>();
        ShowBanner = !ConsentFeature?.CanTrack ?? false;
        CookieString = ConsentFeature?.CreateConsentCookie() ?? "";
    }


    private async Task AcceptCookie()
    {
        await JSRuntime.InvokeVoidAsync("Website.General.acceptCookie", CookieString).ConfigureAwait(false);
        ShowBanner = false;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }
}
