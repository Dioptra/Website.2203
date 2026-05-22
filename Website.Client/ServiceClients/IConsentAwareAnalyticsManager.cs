namespace Website.Client;

public interface IConsentAwareAnalyticsManager
{
    Task TrackEvent(string eventName);
}
