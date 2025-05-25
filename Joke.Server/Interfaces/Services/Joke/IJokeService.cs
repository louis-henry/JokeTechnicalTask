using Joke.Shared.Models.Joke;

namespace Joke.Server.Interfaces.Services.Joke;

/// <summary>
/// Provides core joke operations including sending, receiving, and fetching jokes.
/// </summary>
public interface IJokeService
{
    /// <summary>
    /// Starts the background task responsible for sending jokes at scheduled intervals or under specific conditions.
    /// </summary>
    /// <param name="cancellationToken">Token used to signal task cancellation.</param>
    /// <returns>A task representing the asynchronous sender loop.</returns>
    Task RunSenderTaskAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Handles a joke received from an external source, such as a server or another client.
    /// </summary>
    /// <param name="receivedJoke">The received <see cref="JokeEntity"/>, or <c>null</c> if reception failed.</param>
    /// <param name="isSuccess">Indicates whether the reception was successful.</param>
    /// <returns>A task representing the asynchronous receive operation.</returns>
    Task ReceiveJokeAsync(JokeEntity? receivedJoke, bool isSuccess = true);

    /// <summary>
    /// Fetches a batch of jokes from an external data source or repository.
    /// </summary>
    /// <param name="cancellationToken">Token used to signal cancellation.</param>
    /// <returns>A task representing the asynchronous fetch operation.</returns>
    Task FetchJokesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sends a joke to the target destination (e.g., server, UI component, or connected client).
    /// </summary>
    /// <param name="cancellationToken">Token used to signal cancellation.</param>
    /// <returns>A task representing the asynchronous send operation.</returns>
    Task SendJokeAsync(CancellationToken cancellationToken);
}