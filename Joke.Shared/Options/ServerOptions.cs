namespace Joke.Shared.Options;

public class ServerOptions
{
    public const string PropertyName = "Server";
    public string Url { get; init; } = "http://localhost:5044/";
    public string WebSocketEndpoint { get; init; } = "ws";
    public string FullUrl => Url + WebSocketEndpoint;
}