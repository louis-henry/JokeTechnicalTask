using Joke.Shared.Models.Joke;

namespace Joke.Client.Interfaces.Handlers.Joke;

/// <summary>
/// Defines a contract for handling joke-related operations, including starting, stopping,
/// and processing individual joke messages.
/// </summary>
public interface IJokeHandler
{
    /// <summary>
    /// Starts the joke handler and establishes any required connections or listeners.
    /// </summary>
    /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource"/> used to control cancellation for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
    Task<bool> StartAsync(CancellationTokenSource cancellationTokenSource);

    /// <summary>
    /// Stops the joke handler and releases any resources. Optionally exits the application.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe while stopping the handler.</param>
    /// <param name="exitApp">Indicates whether the application should exit after stopping. Default is <c>true</c>.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    Task StopAsync(CancellationToken cancellationToken, bool exitApp = true);

    /// <summary>
    /// Processes a received joke message, including any transformations and/or translations.
    /// </summary>
    /// <param name="message">The joke message to process.</param>
    /// <param name="cancellationToken">The cancellation token to observe during message processing.</param>
    /// <returns>A task that represents the asynchronous message processing operation.</returns>
    Task ProcessMessageAsync(JokeEntity message, CancellationToken cancellationToken);
}