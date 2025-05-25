using Joke.Shared.Models.Joke;

namespace Joke.Client.Interfaces.Handlers.Joke;

public interface IJokeHandler
{
    Task<bool> StartAsync();
    Task StopAsync(bool exitApp = true);
    Task ProcessMessageAsync(JokeEntity message, CancellationToken cancellationToken);
}