using Blazored.LocalStorage;
using Material.Blazor;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Website.Lib;
using Website.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching();

// Add services to the container.
builder.Services.AddRazorPages();

#if BLAZOR_SERVER

builder.Services.AddServerSideBlazor();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

#endif

// Needed for prerendering on WebAssembly as well as general use
builder.Services.AddScoped<INotification, ServerNotificationService>();

builder.Services.AddMBServices();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Has Pentest fixes
    options.CheckConsentNeeded = context => true;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddOptions();
// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddBlazoredLocalStorage();

// Pentest fix
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.AddServerHeader = false;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Potentially omit to avoid CRIME and BREACH attacks - https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-6.0#compression-with-https
//app.UseResponseCompression();

app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseRouting();

// Limit api calls to 10 in a second to prevent external denial of service.
//app.UseRateLimiter(new()
//{
//    GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
//    {
//        if (!context.Request.Path.StartsWithSegments("/api"))
//        {
//            return RateLimitPartition.GetNoLimiter("NoLimit");
//        }

//        return RateLimitPartition.GetFixedWindowLimiter("GeneralLimit",
//            _ => new FixedWindowRateLimiterOptions()
//            {
//                Window = TimeSpan.FromSeconds(1),
//                PermitLimit = 1,
//                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
//                QueueLimit = 10,
//            });
//    }),
//    RejectionStatusCode = 429,
//});

app.MapControllers();

#if BLAZOR_SERVER
app.MapBlazorHub();
#else
app.UseBlazorFrameworkFiles();
#endif

app.UseStaticFiles();

app.MapFallbackToPage("/Host");

app.MapGet("/sitemap.xml", async context =>
{
    await Sitemap.Generate(context);
});

app.Run();
