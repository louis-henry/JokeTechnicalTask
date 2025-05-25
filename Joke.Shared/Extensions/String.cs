using System.Text.Json;

namespace Joke.Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convert string.IsNullOrEmpty into extension
    /// </summary>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    /// <summary>
    /// Checks if a string is a valid JSON string.
    /// </summary>
    public static bool IsJson(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return false;

        try
        {
            JsonDocument.Parse(str);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Sanitise a string from RAW LLM JSON output as it often contains markdown characters
    /// </summary>
    public static string SanitiseFromMarkdown(this string? input)
    {
        if (input is null)
            return string.Empty;
        
        return input.StartsWith("```json") ? input.Replace("```json", "").Replace("```", "").Trim() : input;
    }
    
    /// <summary>
    /// Returns the base directory of the solution
    /// </summary>
    public static DirectoryInfo? GetBaseDirectory(this string info)
    {
        return Directory.GetParent(info)?.Parent?.Parent?.Parent?.Parent;
    }
}