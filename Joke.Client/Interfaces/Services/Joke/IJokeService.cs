using Joke.Shared.Models.Joke;

namespace Joke.Client.Interfaces.Services.Joke;

public interface IJokeService
{
    Task<JokeEntity?> TranslateJokeAsync(JokeEntity joke, CancellationToken cancellationToken);
}