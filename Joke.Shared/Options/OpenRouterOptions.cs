namespace Joke.Shared.Options;

public class OpenRouterOptions
{
    public const string PropertyName = "OpenRouter";
    public string ApiKey { get; init; } = string.Empty;
    public string Endpoint { get; init; } = "https://openrouter.ai/api/v1/chat/completions";
    public string QueryModel { get; init; } = "openai";
    public bool IsTestMode { get; init; } = true;
    public int TestDelay { get; init; }
}