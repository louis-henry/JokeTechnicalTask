namespace Joke.Shared.Interfaces.Services.Http;

/// <summary>
/// Defines an HTTP service for sending HTTP requests.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Sends a POST request to the specified URL with a JSON payload and optional headers.
    /// </summary>
    /// <param name="url">The target URL for the POST request.</param>
    /// <param name="payload">The JSON payload to send in the request body.</param>
    /// <param name="headers">Optional headers to include in the request.</param>
    /// <returns>A task representing the asynchronous operation, with a string result containing the response content.</returns>
    Task<string> PostAsync(string url, string payload, Dictionary<string, string>? headers = null);
}