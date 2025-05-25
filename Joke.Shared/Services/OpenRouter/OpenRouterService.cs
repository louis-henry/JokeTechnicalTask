using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Models.Prompt.Request;

namespace Joke.Shared.Services.OpenRouter;

public class OpenRouterService : IOpenRouterService
{
    public async Task<T?> QueryModelAsync<T>(PromptRequest request)
    {
        await Task.Yield();
        // TODO
        return default;
    }
    private Dictionary<string, string> GetDefaultHeaders()
    {
        return new Dictionary<string, string>
        {
            { "Authorization", "Bearer API_KEY_TODO" },
            { "HTTP-Referer", "http://localhost" },
            { "X-Title", "Query LLM via OpenRouter" }
        };
    }
}