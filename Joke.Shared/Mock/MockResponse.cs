using Joke.Shared.Extensions;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Response;

namespace Joke.Shared.Mock;

public static class MockResponse
{
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