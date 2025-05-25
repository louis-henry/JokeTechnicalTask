using Joke.Shared.Models.Joke;
using Joke.Shared.Types.Connection;

namespace Joke.Server.Interfaces.Services.Joke;

public interface IJokeViewService
{
    event Action? JokesChanged;
    event Action? SentJokesChanged;
    event Action? TranslatedJokesChanged;
    event Action? ConnectionStatusChanged;
    IEnumerable<JokeEntity> GetJokes();
    IEnumerable<JokeEntity> GetSentJokes();
    IEnumerable<JokeEntity> GetTranslatedJokes();
    ConnectionStatus GetConnectionStatus();
    void EmitJokesChange();
    void EmitSentJokesChange();
    void EmitTranslatedJokesChange();
    void EmitConnectionStatusChangeChange(ConnectionStatus newStatus);
}