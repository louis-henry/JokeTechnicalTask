using Joke.Shared.Models.Joke;

namespace Joke.Server.Interfaces.Services.Joke;

/// <summary>
/// Defines a service responsible for sending jokes and emitting related events to notify other components.
/// </summary>
public interface IJokeSenderService
{
    /// <summary>
    /// Sends a single joke to the target destination (e.g., client, server, or external service).
    /// </summary>
    /// <param name="joke">The <see cref="JokeEntity"/> to be sent.</param>
    /// <returns>A task representing the asynchronous send operation.</returns>
    Task SendJokeAsync(JokeEntity joke);

    /// <summary>
    /// Emits an event indicating that a batch of jokes has been fetched.
    /// </summary>
    void EmitFetchedJokes();

    /// <summary>
    /// Emits an event indicating that a joke has been successfully sent.
    /// </summary>
    void EmitSentJoke();

    /// <summary>
    /// Emits an event indicating that a joke has been received by the client.
    /// </summary>
    void EmitReceivedJoke();
}