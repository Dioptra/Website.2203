using Microsoft.Extensions.DependencyInjection;

namespace Website.Client;

public static class CookieConsentServiceCollectionExtensions
{
    public static IServiceCollection AddCookieConsentSupport(this IServiceCollection services)
    {
        services.AddScoped<ICookieConsentStore, CookieConsentStore>();
        services.AddSingleton<ICookieConsentBannerController, CookieConsentBannerController>();
        services.AddScoped<IConsentAwareAnalyticsManager, ConsentAwareAnalyticsManager>();

        return services;
    }
}
