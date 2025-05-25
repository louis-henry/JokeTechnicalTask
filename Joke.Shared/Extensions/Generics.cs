using System.Text.Json;

namespace Joke.Shared.Extensions;

public static class GenericsExtensions
{
    /// <summary>
    /// Adds an extensions to generic T types to serialize using protection.
    /// </summary>
    public static string SerializeT<T>(this T input, JsonSerializerOptions? options = null)
    {
        try
        {
            return options is null 
                ? JsonSerializer.Serialize(input)
                : JsonSerializer.Serialize(input, options);
        }
        catch
        {
            return string.Empty;
        }
    }
    
    /// <summary>
    /// Adds an extensions to generic T types to deserialize using protection.
    /// </summary>
    /// <returns></returns>
    public static T? DeserializeT<T>(this string input, JsonSerializerOptions? options = null)
    {
        try
        {
            return options is null 
                ? JsonSerializer.Deserialize<T>(input)
                : JsonSerializer.Deserialize<T>(input, options);
        }
        catch
        {
            return default;
        }
    }
}