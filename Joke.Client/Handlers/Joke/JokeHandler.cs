using Joke.Client.Interfaces.Handlers.Joke;

namespace Joke.Client.Handlers.Joke;

public class JokeHandler : IJokeHandler
{
    public async Task StartAsync()
    {
        await Task.Yield();
        // TODO
    }

    public async Task StopAsync()
    {
        await Task.Yield();
        // TODO
    }
}