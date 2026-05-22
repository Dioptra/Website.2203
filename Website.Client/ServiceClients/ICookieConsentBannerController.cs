namespace Website.Client;

public interface ICookieConsentBannerController
{
    event Action? ShowRequested;

    void RequestShow();
}
