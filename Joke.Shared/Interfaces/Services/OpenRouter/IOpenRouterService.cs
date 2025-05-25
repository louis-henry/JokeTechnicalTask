using Joke.Shared.Models.Prompt.Request;

namespace Joke.Shared.Interfaces.Services.OpenRouter;

public interface IOpenRouterService
{
    Task<T?> QueryModelAsync<T>(PromptRequest request);
}