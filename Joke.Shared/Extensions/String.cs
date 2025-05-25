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
    /// Returns the base directory of the solution
    /// </summary>
    public static DirectoryInfo? GetBaseDirectory(this string info)
    {
        return Directory.GetParent(info)?.Parent?.Parent?.Parent?.Parent;
    }
}