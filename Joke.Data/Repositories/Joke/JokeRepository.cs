using Joke.Data.Interfaces.Repositories.Joke;
using Joke.Shared.Models.Joke;

namespace Joke.Data.Repositories.Joke;

public class JokeRepository : IJokeRepository
{
    private readonly List<JokeEntity> _jokes = [];
    private readonly List<JokeEntity> _sentJokes = [];
    private readonly List<JokeEntity> _translatedJokes = [];
    
    public IEnumerable<JokeEntity> GetJokes() => _jokes;
    public IEnumerable<JokeEntity> GetSentJokes() => _sentJokes;
    public IEnumerable<JokeEntity> GetTranslatedJokes() =>  _translatedJokes;
    public JokeEntity? GetJoke() => _jokes.Count > 0 ? _jokes.FirstOrDefault() : null;
    public int GetSentJokesCount() => _sentJokes.Count;
    public void AddJokes(IEnumerable<JokeEntity> newJokes) => _jokes.AddRange(newJokes);
    public void MarkSentJokes(IEnumerable<Guid> ids)
    {
        ids.ToList().ForEach(id =>
        {
            var target = _jokes.FirstOrDefault(x => x.Id == id);
            if (target is null) return;
            
            _sentJokes.Add(target);
            _jokes.Remove(target);
        });
    }
    public void AddTranslatedJokes(IEnumerable<JokeEntity> translatedJokes)
    {
        var jokesAsList = translatedJokes.ToList();
        jokesAsList.ForEach(joke =>
        {
            var target = _sentJokes.FirstOrDefault(x => x.Id == joke.Id);
            if (target is null) return;
            
            _sentJokes.Remove(target);
        });
        _translatedJokes.AddRange(jokesAsList);
    }
}