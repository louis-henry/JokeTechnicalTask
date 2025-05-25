using Joke.Shared.Models.Joke;

namespace Joke.Client.Interfaces.Handlers.Joke;

public interface IJokeHandler
{
    Task<bool> StartAsync(CancellationTokenSource cancellationTokenSource);
    Task StopAsync(CancellationToken cancellationToken, bool exitApp = true);
    Task ProcessMessageAsync(JokeEntity message, CancellationToken cancellationToken);
}