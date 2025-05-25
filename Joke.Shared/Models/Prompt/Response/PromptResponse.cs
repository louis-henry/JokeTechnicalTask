using System.Text.Json.Serialization;

namespace Joke.Shared.Models.Prompt.Response;

public class PromptResponse
{
    public required string Id { get; set; }
    public required string Provider { get; set; }
    public required string Model { get; set; }
    [JsonPropertyName("object")]
    public required string ObjectType { get; set; }
    public long Created { get; set; }
    public List<PromptResponseChoice> Choices { get; set; } = [];
    public PromptResponseUsage? Usage { get; set; }
}