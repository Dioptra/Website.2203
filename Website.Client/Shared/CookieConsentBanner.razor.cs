using Microsoft.AspNetCore.Components;

namespace Website.Client;

/// <summary>
/// Shows a cookie consent banner.
/// </summary>
public partial class CookieConsentBanner : ComponentBase
{
    [Inject] private ICookieConsentBannerController BannerController { get; set; } = default!;
    [Inject] private ICookieConsentStore ConsentStore { get; set; } = default!;


    /// <summary>
    /// Optional color CSS class.
    /// </summary>
    [Parameter] public string ColorClass { get; set; } = "";

    private bool AllowAnalytics { get; set; } = true;

    private bool ShowBanner { get; set; }


    protected override void OnInitialized()
    {
        BannerController.ShowRequested += OnShowRequested;
    }


    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var consent = await ConsentStore.GetAsync().ConfigureAwait(false);

            if (consent is null)
            {
                ShowBanner = true;
            }
            else
            {
                AllowAnalytics = consent.Analytics;
            }

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }


    public void Dispose()
    {
        BannerController.ShowRequested -= OnShowRequested;
    }


    private void OnShowRequested()
    {
        ShowBanner = true;
        _ = InvokeAsync(StateHasChanged);
    }


    private async Task AcceptAllAsync()
    {
        AllowAnalytics = true;
        await SaveAsync().ConfigureAwait(false);
    }


    private async Task RejectNonEssentialAsync()
    {
        AllowAnalytics = false;
        await SaveAsync().ConfigureAwait(false);
    }


    private async Task SavePreferencesAsync()
    {
        await SaveAsync().ConfigureAwait(false);
    }


    private async Task SaveAsync()
    {
        await ConsentStore.SaveAsync(new CookieConsentSnapshot(CookieConsentConstants.CurrentVersion, AllowAnalytics, DateTimeOffset.UtcNow)).ConfigureAwait(false);
        ShowBanner = false;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }
}
