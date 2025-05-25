using Joke.Shared.Models.Joke;

namespace Joke.Data.Interfaces.Repositories.Joke;

public interface IJokeRepository
{
    JokeEntity? GetJoke();
    IEnumerable<JokeEntity> GetJokes();
    IEnumerable<JokeEntity> GetSentJokes();
    IEnumerable<JokeEntity> GetTranslatedJokes();
    int GetSentJokesCount();
    void AddJokes(IEnumerable<JokeEntity> newJokes);
    void MarkSentJokes(IEnumerable<Guid> ids);
    void AddTranslatedJokes(IEnumerable<JokeEntity> translatedJokes);
}