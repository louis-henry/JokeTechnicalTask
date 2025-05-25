namespace Joke.Shared.Interfaces.Services.Http;

public interface IHttpService
{
    Task<string> PostAsync(string url, string payload, Dictionary<string, string>? headers = null);
}