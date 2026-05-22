using Blazored.LocalStorage;
using GoogleAnalytics.Blazor;
using Material.Blazor;
using Website.Client;
using Website.Components;

var builder = WebApplication.CreateBuilder(args);

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
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Website.Client.Routes).Assembly);

app.Run();
