using Blazored.LocalStorage;
using GoogleAnalytics.Blazor;
using Material.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Website.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<INotification, WebAssemblyNotificationService>();
builder.Services.AddMBServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCookieConsentSupport();
builder.Services.AddGBService(options =>
{
    options.TrackingId = "G-V061TDSPDR";
    options.GlobalEventParams = new Dictionary<string, object>
    {
        { Utilities.EventCategory, Utilities.DialogActions },
        { Utilities.NonInteraction, true },
    };
});

await builder.Build().RunAsync();
