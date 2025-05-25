using System.Text;
using Joke.Shared.Extensions;
using Joke.Shared.Interfaces.Services.Http;
using Microsoft.Extensions.Logging;

namespace Joke.Shared.Services.Http;

public class HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger) : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentException(nameof(httpClientFactory));
    private readonly ILogger<HttpService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    
    public async Task<string> PostAsync(string url, string payload, Dictionary<string, string>? headers = null)
    {
        _logger.LogDebug("{@method} called", nameof(PostAsync));
        _logger.LogTrace("{@method} with payload: {@payload}, url: {@url}",
            nameof(PostAsync), payload, url);
        
        if (url.IsNullOrEmpty() || payload.IsNullOrEmpty())
        {
            _logger.LogWarning("{@method} missing minimum request parameters. Payload: {@payload}, Url: {@url}", 
                nameof(PostAsync), payload, url);
            return string.Empty;
        }
        
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            AddHeaders(httpClient, headers);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("{@method} warning - http request failed with code: {@code}",
                    nameof(PostAsync), response.StatusCode);
                return string.Empty;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error - http call failed", nameof(PostAsync));
            throw;
        }
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