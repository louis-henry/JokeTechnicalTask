using Joke.Shared.Models.Joke;
using Joke.Shared.Types.Connection;

namespace Joke.Server.Interfaces.Services.Joke;

/// <summary>
/// Provides access to joke-related view data and emits change notifications for UI updates or other listeners.
/// </summary>
public interface IJokeViewService
{
    /// <summary>
    /// Occurs when the collection of available jokes changes.
    /// </summary>
    event Action? JokesChanged;

    /// <summary>
    /// Occurs when the collection of sent jokes changes.
    /// </summary>
    event Action? SentJokesChanged;

    /// <summary>
    /// Occurs when the collection of translated jokes changes.
    /// </summary>
    event Action? TranslatedJokesChanged;

    /// <summary>
    /// Occurs when the connection status changes.
    /// </summary>
    event Action? ConnectionStatusChanged;

    /// <summary>
    /// Gets the current collection of available jokes.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="JokeEntity"/>.</returns>
    IEnumerable<JokeEntity> GetJokes();

    /// <summary>
    /// Gets the collection of jokes that have been sent.
    /// </summary>
    /// <returns>An enumerable collection of sent <see cref="JokeEntity"/>.</returns>
    IEnumerable<JokeEntity> GetSentJokes();

    /// <summary>
    /// Gets the collection of jokes that have been translated.
    /// </summary>
    /// <returns>An enumerable collection of translated <see cref="JokeEntity"/>.</returns>
    IEnumerable<JokeEntity> GetTranslatedJokes();

    /// <summary>
    /// Gets the current connection status.
    /// </summary>
    /// <returns>A <see cref="ConnectionStatus"/> value representing the current state.</returns>
    ConnectionStatus GetConnectionStatus();

    /// <summary>
    /// Triggers the <see cref="JokesChanged"/> event to notify subscribers of joke updates.
    /// </summary>
    void EmitJokesChange();

    /// <summary>
    /// Triggers the <see cref="SentJokesChanged"/> event to notify subscribers of sent joke updates.
    /// </summary>
    void EmitSentJokesChange();

    /// <summary>
    /// Triggers the <see cref="TranslatedJokesChanged"/> event to notify subscribers of translated joke updates.
    /// </summary>
    void EmitTranslatedJokesChange();

    /// <summary>
    /// Triggers the <see cref="ConnectionStatusChanged"/> event and updates the connection status.
    /// </summary>
    /// <param name="newStatus">The new <see cref="ConnectionStatus"/> to set.</param>
    void EmitConnectionStatusChangeChange(ConnectionStatus newStatus);
}