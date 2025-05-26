using System.Text.Json.Serialization;

namespace Joke.Shared.Models.Prompt.Request;

public class PromptRequest
{
    [JsonIgnore]
    public string? Id { get; init; }
    public required string Model { get; set; }
    public IEnumerable<PromptRequestMessage> Messages { get; set; } = [];
}