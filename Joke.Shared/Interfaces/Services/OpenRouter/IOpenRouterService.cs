using Joke.Shared.Models.Prompt.Request;

namespace Joke.Shared.Interfaces.Services.OpenRouter;

/// <summary>
/// Defines a service interface for querying an external model or API with prompt requests.
/// </summary>
public interface IOpenRouterService
{
    /// <summary>
    /// Asynchronously sends a prompt request to the external model and retrieves the response.
    /// </summary>
    /// <param name="request">The prompt request to be sent to the model.</param>
    /// <returns>
    /// A task representing the asynchronous operation.  
    /// The task result contains an <see cref="T"/> object with the response data, or <c>null</c> if no valid response was received.
    /// </returns>
    /// <remarks>
    /// Implementations should handle communication with the external model or API, including serialization and deserialization of the request and response.
    /// Exceptions should be handled or propagated as appropriate.
    /// </remarks>
    Task<T?> QueryModelAsync<T>(PromptRequest request);
}