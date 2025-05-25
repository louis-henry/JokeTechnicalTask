using Joke.Shared.Interfaces.Services.Http;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Options;
using Joke.Shared.Services.Http;
using Joke.Shared.Services.OpenRouter;
using Radzen;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services
    .AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.EnableDetailedErrors = true;
    });

// Add SignalR
builder.Services.AddSignalR();

// Add HttpClient
builder.Services.AddHttpClient();

// Add Services
builder.Services
    .AddScoped<IOpenRouterService, OpenRouterService>()
    .AddScoped<IHttpService, HttpService>();

// Add Repositories
// TODO

// Add Radzen UI
builder.Services.AddRadzenComponents();

// AppSettings as IOptions<T>
builder.Services.Configure<OpenRouterOptions>(builder.Configuration.GetSection(OpenRouterOptions.PropertyName));

var app = builder.Build();

// Default setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapBlazorHub();

// Map endpoints via top level routing
app.MapFallbackToPage("/_Host");

// Run
Log.Logger.Information("Listening at http://localhost:5044");
app.Run();