namespace Website.Client;

public sealed class CookieConsentBannerController : ICookieConsentBannerController
{
    public event Action? ShowRequested;

    public void RequestShow()
    {
        ShowRequested?.Invoke();
    }
}
