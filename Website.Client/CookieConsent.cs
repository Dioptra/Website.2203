namespace Website.Client;

public static class CookieConsentConstants
{
    public const string CookieName = "Website.CookieConsent";
    public const string LocalStorageKey = CookieName;
    public const int CurrentVersion = 1;
    public const int CookieMaxAgeDays = 180;
}

public sealed record CookieConsentSnapshot(int Version, bool Analytics, DateTimeOffset UpdatedUtc);
