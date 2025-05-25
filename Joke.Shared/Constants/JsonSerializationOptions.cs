using System.Text.Json;
using System.Text.Json.Serialization;

namespace Joke.Shared.Constants;

/// <summary>
/// Common JSON options used throughout both Server and Client. Preferable
/// over global JSON options as different external services require different settings
/// (i.e., camel_case vs. PascalCase)
/// </summary>
public static class JsonOptions
{
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}