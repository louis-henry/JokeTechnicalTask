using Joke.Shared.Models.Joke;

namespace Joke.Data.Interfaces.Repositories.Joke;

/// <summary>
/// Defines methods for managing joke storage, retrieval, and state tracking within the application.
/// </summary>
public interface IJokeRepository
{
    /// <summary>
    /// Retrieves a single joke from the repository.
    /// </summary>
    /// <returns>A <see cref="JokeEntity"/> instance, or <c>null</c> if none are available.</returns>
    JokeEntity? GetJoke();

    /// <summary>
    /// Retrieves all jokes currently stored in the repository.
    /// </summary>
    /// <returns>An enumerable collection of all <see cref="JokeEntity"/> instances.</returns>
    IEnumerable<JokeEntity> GetJokes();

    /// <summary>
    /// Retrieves all jokes that have been marked as sent.
    /// </summary>
    /// <returns>An enumerable collection of sent <see cref="JokeEntity"/> instances.</returns>
    IEnumerable<JokeEntity> GetSentJokes();

    /// <summary>
    /// Retrieves all jokes that have been translated.
    /// </summary>
    /// <returns>An enumerable collection of translated <see cref="JokeEntity"/> instances.</returns>
    IEnumerable<JokeEntity> GetTranslatedJokes();

    /// <summary>
    /// Gets the total number of jokes that have been sent.
    /// </summary>
    /// <returns>The number of sent jokes.</returns>
    int GetSentJokesCount();

    /// <summary>
    /// Adds a collection of new jokes to the repository.
    /// </summary>
    /// <param name="newJokes">The jokes to be added.</param>
    void AddJokes(IEnumerable<JokeEntity> newJokes);

    /// <summary>
    /// Marks the specified jokes as sent using their unique identifiers.
    /// </summary>
    /// <param name="ids">The IDs of the jokes to mark as sent.</param>
    void MarkSentJokes(IEnumerable<Guid> ids);

    /// <summary>
    /// Removes the specified jokes from the list of sent jokes.
    /// </summary>
    /// <param name="jokes">The jokes to be removed from the sent collection.</param>
    void RemoveSentJokes(IEnumerable<JokeEntity> jokes);

    /// <summary>
    /// Adds a collection of translated jokes to the repository.
    /// </summary>
    /// <param name="translatedJokes">The translated jokes to be added.</param>
    void AddTranslatedJokes(IEnumerable<JokeEntity> translatedJokes);
}