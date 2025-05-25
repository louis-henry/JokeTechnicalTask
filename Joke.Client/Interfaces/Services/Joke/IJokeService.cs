using Joke.Shared.Models.Joke;

namespace Joke.Client.Interfaces.Services.Joke;

/// <summary>
/// Provides joke-related services such as translating or transforming joke content.
/// </summary>
public interface IJokeService
{
    /// <summary>
    /// Translates the given joke into another language or format.
    /// </summary>
    /// <param name="joke">The joke entity to be translated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the translated <see cref="JokeEntity"/>, or <c>null</c> if the translation failed.
    /// </returns>
    Task<JokeEntity?> TranslateJokeAsync(JokeEntity joke, CancellationToken cancellationToken);
}