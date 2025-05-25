using Joke.Client.Interfaces.Handlers.Joke;
using Joke.Client.Interfaces.Services.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace Joke.Client.Services.Menu;

public class MenuService(IServiceProvider serviceProvider) : IMenuService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentException(nameof(serviceProvider));
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
        Console.ReadKey();
        await DisconnectFromWebSocketAsync();
    }
    
    private async Task<bool> ConnectToWebSocketAsync()
    {
        var handler = _serviceProvider.GetRequiredService<IJokeHandler>();
        return await handler.StartAsync();
    }
    
    private async Task DisconnectFromWebSocketAsync()
    {
        var handler = _serviceProvider.GetRequiredService<IJokeHandler>();
        await handler.StopAsync();
    }
}