using Joke.Data.Interfaces.Repositories.Joke;
using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Models.Joke;
using Joke.Shared.Types.Connection;

namespace Joke.Server.Services.Joke;

public class JokeViewService(IJokeRepository jokeRepository) : IJokeViewService
{
    public event Action? JokesChanged;
    public event Action? SentJokesChanged;
    public event Action? TranslatedJokesChanged;
    public event Action? ConnectionStatusChanged;

    private readonly IJokeRepository _jokeRepository = jokeRepository ?? throw new ArgumentException(nameof(jokeRepository));
    private ConnectionStatus _connectionStatus = ConnectionStatus.Awaiting;
    
    public IEnumerable<JokeEntity> GetJokes() => _jokeRepository.GetJokes();
    public IEnumerable<JokeEntity> GetSentJokes() => _jokeRepository.GetSentJokes();
    public IEnumerable<JokeEntity> GetTranslatedJokes() => _jokeRepository.GetTranslatedJokes();
    public ConnectionStatus GetConnectionStatus() => _connectionStatus;
    public void EmitJokesChange() => JokesChanged?.Invoke();
    public void EmitSentJokesChange() => SentJokesChanged?.Invoke();
    public void EmitTranslatedJokesChange() => TranslatedJokesChanged?.Invoke();
    public void EmitConnectionStatusChangeChange(ConnectionStatus newStatus)
    {
        _connectionStatus = newStatus;
        ConnectionStatusChanged?.Invoke();
    }
}