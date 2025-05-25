namespace Joke.Shared.Models.Prompt.Request;

public class PromptRequestMessage
{
    public required string Role { get; set; }
    public required string Content { get; set; }
}