using Joke.Client.Interfaces.Services.Menu;

namespace Joke.Client.Services.Menu;

public class MenuService : IMenuService
{
    public async Task RunAsync()
    {
        await Task.Yield();
        Console.WriteLine("***************** Joke Client ******************");
        Console.WriteLine("Press any key to close");
        Console.ReadKey();
    }
}