namespace Joke.Shared.Models.Prompt.Response;

public class PromptResponseMessage
{
    public required string Role { get; set; }
    public required string Content { get; set; }
    public object? Refusal { get; set; }
    public object? Reasoning { get; set; }
}