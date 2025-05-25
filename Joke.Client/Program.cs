using Joke.Shared.Options;
using Joke.Client.Handlers.Joke;
using Joke.Client.Interfaces.Handlers.Joke;
using Joke.Client.Interfaces.Services.Joke;
using Joke.Client.Interfaces.Services.Menu;
using Joke.Client.Services.Joke;
using Joke.Client.Services.Menu;
using Joke.Shared.Interfaces.Services.Http;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Services.Http;
using Joke.Shared.Services.OpenRouter;
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
    .AddScoped<IOpenRouterService, OpenRouterService>()
    .AddScoped<IHttpService, HttpService>()
    .AddSingleton<IMenuService, MenuService>()
    .AddSingleton<IJokeHandler, JokeHandler>();

// AppSettings as IOptions<T>
builder.Services.Configure<OpenRouterOptions>(builder.Configuration.GetSection(OpenRouterOptions.PropertyName));
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection(ServerOptions.PropertyName));

// Start
var host = builder.Build();
var menu = host.Services.GetRequiredService<IMenuService>();
await menu.RunAsync();