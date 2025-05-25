using Joke.Client.Interfaces.Handlers.Joke;
using Joke.Client.Interfaces.Services.Menu;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Joke.Client.Services.Menu;

public class MenuService(IServiceProvider serviceProvider, ILogger<MenuService> logger) : IMenuService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentException(nameof(serviceProvider));
    private readonly ILogger<MenuService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    
    public async Task RunAsync()
    {
        await Task.Yield();
        Console.WriteLine("***************** Joke Client ******************");
        Console.WriteLine("Press any key to open a connection with the server");
        Console.ReadKey();
        if (!await ConnectToWebSocketAsync())
        {
            Console.WriteLine("Exiting...");
            return;
        }
        
        Console.WriteLine("Press any key to disconnect");
        await WaitForKeyPressOrCancellationAsync(_cts.Token);

        if (!_cts.IsCancellationRequested)
        {
            // Attempt to disconnect manually when user cancelled
            await DisconnectFromWebSocketAsync();
        }
    }
    
    private async Task<bool> ConnectToWebSocketAsync()
    {
        var handler = _serviceProvider.GetRequiredService<IJokeHandler>();
        return await handler.StartAsync(_cts);
    }
    
    private async Task DisconnectFromWebSocketAsync()
    {
        var handler = _serviceProvider.GetRequiredService<IJokeHandler>();
        await handler.StopAsync(_cts.Token);
    }
    
    private async Task WaitForKeyPressOrCancellationAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    return;
                }

                await Task.Delay(100, token);
            }
        }
        catch(TaskCanceledException)
        {
            _logger.LogInformation("{@method} CancellationToken is cancelled", nameof(WaitForKeyPressOrCancellationAsync));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "{@method} Message operation cancelled", nameof(WaitForKeyPressOrCancellationAsync));
        }
    }
}