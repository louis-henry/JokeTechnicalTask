using Joke.Shared.Models.Joke;

namespace Joke.Server.Interfaces.Services.Joke;

public interface IJokeService
{
    Task RunSenderTaskAsync(CancellationToken cancellationToken);
    Task ReceiveJokeAsync(JokeEntity? receivedJoke);
    Task FetchJokesAsync(CancellationToken cancellationToken);
    Task SendJokeAsync(CancellationToken cancellationToken);
}