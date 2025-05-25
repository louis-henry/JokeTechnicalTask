using Joke.Shared.Interfaces.Services.Http;

namespace Joke.Shared.Services.Http;

public class HttpService : IHttpService
{
    public async Task<string> PostAsync(string url, string payload, Dictionary<string, string>? headers = null)
    {
        await Task.Yield();
        // TODO
        return string.Empty;
    }
    private static void AddHeaders(HttpClient httpClient, Dictionary<string, string>? headers)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var header in headers)
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }
}