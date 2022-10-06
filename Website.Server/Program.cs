using Material.Blazor;
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

builder.Services.AddMBServices();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddOptions();
// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

#if BLAZOR_SERVER
app.MapBlazorHub();
#else
app.UseBlazorFrameworkFiles();
#endif

app.UseStaticFiles();

app.MapFallbackToPage("/Host");

app.Run();
