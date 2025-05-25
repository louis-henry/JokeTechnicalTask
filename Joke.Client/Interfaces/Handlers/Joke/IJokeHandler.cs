namespace Joke.Client.Interfaces.Handlers.Joke;

public interface IJokeHandler
{
    Task StartAsync();
    Task StopAsync();
}