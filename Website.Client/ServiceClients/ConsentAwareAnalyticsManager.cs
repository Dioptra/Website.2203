using GoogleAnalytics.Blazor;

namespace Website.Client;

public sealed class ConsentAwareAnalyticsManager : IConsentAwareAnalyticsManager
{
    private readonly ICookieConsentStore cookieConsentStore;
    private readonly IGBAnalyticsManager analyticsManager;

    public ConsentAwareAnalyticsManager(ICookieConsentStore cookieConsentStore, IGBAnalyticsManager analyticsManager)
    {
        this.cookieConsentStore = cookieConsentStore;
        this.analyticsManager = analyticsManager;
    }

    public async Task TrackEvent(string eventName)
    {
        if (!await cookieConsentStore.HasAnalyticsConsentAsync().ConfigureAwait(false))
        {
            return;
        }

        await analyticsManager.TrackEvent(eventName).ConfigureAwait(false);
    }
}
