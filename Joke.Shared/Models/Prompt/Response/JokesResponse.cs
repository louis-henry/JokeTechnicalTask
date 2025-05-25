using Joke.Shared.Models.Joke;

namespace Joke.Shared.Models.Prompt.Response;

public class JokesResponse
{
    public IEnumerable<JokeEntity> Jokes { get; init; } = [];
}