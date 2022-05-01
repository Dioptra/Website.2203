using Blazored.LocalStorage;
using Material.Blazor;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using Serilog.Events;
using Website.Lib;

const string _customTemplate = "{Timestamp: HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}";
const string _loggingWebhook = "https://blacklandcapital.webhook.office.com/webhookb2/6ccfaed1-7c02-440c-83f0-9265cf35b379@ef73a184-f1db-4f24-b406-e4f8f9633dfa/IncomingWebhook/18bed2df0852449aa5d92541255caade/34ba3a07-c6f6-4e3f-896d-148fb6c1765f";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMBServices(loggingServiceConfiguration: Utilities.GetDefaultLoggingServiceConfiguration(), toastServiceConfiguration: Utilities.GetDefaultToastServiceConfiguration(), snackbarServiceConfiguration: Utilities.GetDefaultSnackbarServiceConfiguration());

builder.Services.AddHttpClient();
builder.Services.AddTransient<ITeamsNotificationService, TeamsNotificationService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Has Pentest fixes
    options.CheckConsentNeeded = context => true;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.Configure<StaticFileOptions>(options =>
{
    // Pentest fix
    options.OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        ctx.Context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.Console(outputTemplate: _customTemplate, restrictedToMinimumLevel: LogEventLevel.Information))
    .WriteTo.Conditional(evt => !app.Environment.IsDevelopment(), wt => wt.Async(a => a.MicrosoftTeams(outputTemplate: _customTemplate, webHookUri: _loggingWebhook, titleTemplate: "Dioptra Website", restrictedToMinimumLevel: LogEventLevel.Warning)))
    .WriteTo.Conditional(evt => app.Environment.IsDevelopment(), wt => wt.Async(a => a.File(outputTemplate: _customTemplate, path: Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Dioptra Website\\blazor-server-app.log", restrictedToMinimumLevel: LogEventLevel.Debug, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)))
    .CreateLogger();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy(); 

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

// Pentest fix
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Xss-Protection", "1");
    await next();
});

// Pentest fix
app.UseMiddleware<NoCacheMiddleware>();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapGet("/sitemap.xml", async context => {
    await Sitemap.Generate(context);
});

app.Run();
