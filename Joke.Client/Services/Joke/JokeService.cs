using Joke.Client.Interfaces.Services.Joke;

namespace Joke.Client.Services.Joke;

public class JokeService : IJokeService
{
    public async Task TranslateJokeAsync()
    {
        await Task.Yield();
        // TODO
    }
}