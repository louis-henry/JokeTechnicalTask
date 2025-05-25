using Joke.Client.Interfaces.Services.Joke;
using Joke.Client.Interfaces.Services.Menu;
using Joke.Client.Services.Joke;
using Joke.Client.Services.Menu;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

// Build
var builder = Host.CreateApplicationBuilder(args);

// Add Configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
    
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
    
// Add Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger, dispose: true);

// Add HttpClient
builder.Services.AddHttpClient();

// Add Services
builder.Services
    .AddScoped<IJokeService, JokeService>()
    .AddSingleton<IMenuService, MenuService>();

// Start
var host = builder.Build();
var menu = host.Services.GetRequiredService<IMenuService>();
await menu.RunAsync();