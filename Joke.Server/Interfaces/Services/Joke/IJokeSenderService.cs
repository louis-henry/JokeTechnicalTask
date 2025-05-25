using Joke.Shared.Models.Joke;

namespace Joke.Server.Interfaces.Services.Joke;

public interface IJokeSenderService
{
    Task SendJokeAsync(JokeEntity joke);
    void EmitFetchedJokes();
    void EmitSentJoke();
    void EmitReceivedJoke();
}