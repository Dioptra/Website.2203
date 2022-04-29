using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;

namespace Website.Lib.Shared;
public partial class ConsentCookie : ComponentBase
{
    [Inject] private IHttpContextAccessor Http { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }


    private const string CookieConsentKey = "CookieConsentKey";
    private const string CookiesConsentedValue = "yes";

    private ITrackingConsentFeature ConsentFeature { get; set; }
    private bool ShowBanner { get; set; } = false;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var consentedValue = await LocalStorage.GetItemAsync<string>(CookieConsentKey).ConfigureAwait(false);

            ShowBanner = consentedValue != CookiesConsentedValue;

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }


    private async Task AcceptCookie()
    {
        await LocalStorage.SetItemAsync(CookieConsentKey, CookiesConsentedValue);
        ShowBanner = false;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }
}
