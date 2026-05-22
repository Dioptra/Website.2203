using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Website.Client;

public sealed class CookieConsentStore : ICookieConsentStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IJSRuntime jsRuntime;
    private readonly ILocalStorageService localStorage;

    public CookieConsentStore(IJSRuntime jsRuntime, ILocalStorageService localStorage)
    {
        this.jsRuntime = jsRuntime;
        this.localStorage = localStorage;
    }

    public async Task<CookieConsentSnapshot?> GetAsync()
    {
        try
        {
            var snapshot = await localStorage.GetItemAsync<CookieConsentSnapshot>(CookieConsentConstants.LocalStorageKey).ConfigureAwait(false);

            if (snapshot is not null)
            {
                return snapshot.Version == CookieConsentConstants.CurrentVersion ? snapshot : null;
            }

            var cookieJson = await jsRuntime.InvokeAsync<string?>("Website.General.getCookie", CookieConsentConstants.CookieName).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(cookieJson))
            {
                return null;
            }

            snapshot = JsonSerializer.Deserialize<CookieConsentSnapshot>(cookieJson, JsonOptions);

            if (snapshot is null || snapshot.Version != CookieConsentConstants.CurrentVersion)
            {
                return null;
            }

            await localStorage.SetItemAsync(CookieConsentConstants.LocalStorageKey, snapshot).ConfigureAwait(false);

            return snapshot;
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveAsync(CookieConsentSnapshot consent)
    {
        await localStorage.SetItemAsync(CookieConsentConstants.LocalStorageKey, consent).ConfigureAwait(false);
        await jsRuntime.InvokeVoidAsync("Website.General.setCookie", CookieConsentConstants.CookieName, JsonSerializer.Serialize(consent, JsonOptions), CookieConsentConstants.CookieMaxAgeDays).ConfigureAwait(false);
    }

    public async Task ClearAsync()
    {
        await localStorage.RemoveItemAsync(CookieConsentConstants.LocalStorageKey).ConfigureAwait(false);
        await jsRuntime.InvokeVoidAsync("Website.General.deleteCookie", CookieConsentConstants.CookieName).ConfigureAwait(false);
    }

    public async Task<bool> HasAnalyticsConsentAsync()
    {
        var snapshot = await GetAsync().ConfigureAwait(false);

        return snapshot is not null && snapshot.Analytics && snapshot.Version == CookieConsentConstants.CurrentVersion;
    }
}
