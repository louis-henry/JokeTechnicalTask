using System.Text.Json.Serialization;

namespace Joke.Shared.Models.Joke;

public class JokeEntity
{
    public required string Id { get; set; }
    public required string? Joke { get; set; }
    public string? Answer { get; set; }
    [JsonPropertyName("translated_joke")]
    public string? TranslatedJoke { get; set; }
    [JsonPropertyName("translated_answer")]
    public string? TranslatedAnswer { get; set; }
    [JsonPropertyName("time_to_translate")]
    public long? TimeToTranslate { get; set; }
}