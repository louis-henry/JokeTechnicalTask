using Joke.Shared.Extensions;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Response;

namespace Joke.Shared.Mock;

public static class MockResponse
{
    /// <summary>
    /// Retrieves the contents of a mock data file based on the specified type and optional identifier.
    /// </summary>
    /// <typeparam name="T">The type used to determine which mock file to load (e.g., <c>JokesResponse</c> or <c>JokeEntity</c>).</typeparam>
    /// <param name="id">An optional identifier used to construct the filename, required when the type is <c>JokeEntity</c>.</param>
    /// <returns>
    /// A string containing the file contents if the file is found and readable; otherwise, returns an empty string.
    /// </returns>
    /// <remarks>
    /// The method assumes that mock data files are located in the <c>JC.Shared/MockData</c> directory relative to the application's base directory.
    /// </remarks>
    public static string GetData<T>(string? id = null)
    {
        try
        {
            var fileName = typeof(T).Name switch
            {
                nameof(JokesResponse) => "jokes.json",
                nameof(JokeEntity) => $"{id}.json",
                _ => string.Empty
            };

            if (fileName.IsNullOrEmpty())
                throw new FileNotFoundException();
        
            var fileDirectory = $"{AppContext.BaseDirectory.GetBaseDirectory()}/Joke.Shared/MockData";
            var fileContent = File.ReadAllText(Path.Combine(fileDirectory, fileName));
            return fileContent;
        }
        catch
        {
            return string.Empty;
        }
    }
}