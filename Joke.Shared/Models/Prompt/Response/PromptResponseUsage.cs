namespace Joke.Shared.Models.Prompt.Response;

public class PromptResponseUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}