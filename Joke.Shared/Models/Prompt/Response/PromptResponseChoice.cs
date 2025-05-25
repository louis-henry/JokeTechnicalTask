namespace Joke.Shared.Models.Prompt.Response;

public class PromptResponseChoice
{
    public object? LogProbs { get; set; }
    public string? FinishReason { get; set; }
    public string? NativeFinishReason { get; set; }
    public int Index { get; set; }
    public required PromptResponseMessage Message { get; set; }
}