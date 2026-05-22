namespace Website.Client;

public interface ICookieConsentStore
{
    Task<CookieConsentSnapshot?> GetAsync();

    Task SaveAsync(CookieConsentSnapshot consent);

    Task ClearAsync();

    Task<bool> HasAnalyticsConsentAsync();
}
