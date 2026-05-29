using Blazored.LocalStorage;
using GoogleAnalytics.Blazor;
using Material.Blazor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Website.Client;
using Website.Components;

var startedAtUtc = DateTimeOffset.UtcNow;

var builder = WebApplication.CreateBuilder(args);

const string DefaultOperationalProbeHost = "127.0.0.1";
const int DefaultOperationalProbePort = 8081;

var operationalProbeHost = builder.Configuration["OperationalProbes:Host"] ?? DefaultOperationalProbeHost;
var operationalProbePort = int.TryParse(builder.Configuration["OperationalProbes:Port"], out var configuredOperationalProbePort)
    ? configuredOperationalProbePort
    : DefaultOperationalProbePort;
var operationalProbeAddress = IPAddress.TryParse(operationalProbeHost, out var configuredOperationalProbeAddress)
    ? configuredOperationalProbeAddress
    : IPAddress.Loopback;
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Bind the operational probe listener (default 0.0.0.0:8081 in containers via OperationalProbes__Host)
    // ADDITIVELY, alongside the main app endpoint that comes from Kestrel:Endpoints config
    // (Kestrel__Endpoints__Http__Url=http://+:8080). Using ConfigureKestrel.Listen instead of UseUrls
    // avoids the UseUrls-vs-Kestrel:Endpoints conflict that left port 8081 unbound, breaking estates /livez.
    serverOptions.Listen(operationalProbeAddress, operationalProbePort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddTransient<INotification, Website.ServerNotificationService>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseWhen(context => context.Connection.LocalPort != operationalProbePort, branch =>
{
    branch.UseHttpsRedirection();
});

app.UseAntiforgery();

var operationalProbeGroup = app.MapGroup(string.Empty)
    .RequireHost($"*:{operationalProbePort}");

operationalProbeGroup.MapGet("/livez", () => Results.Ok());
operationalProbeGroup.MapGet("/readyz", () => Results.Ok());
operationalProbeGroup.MapGet("/healthz", () => Results.Json(new
{
    status = "healthy",
    app = "website-2203",
    version = PackageInformation.Version,
    startedAtUtc,
    uptimeSeconds = (long)(DateTimeOffset.UtcNow - startedAtUtc).TotalSeconds,
}));

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Website.Client.Routes).Assembly);

app.Run();
