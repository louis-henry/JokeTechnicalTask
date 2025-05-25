using Joke.Server.Hubs;
using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Models.Joke;
using Microsoft.AspNetCore.SignalR;

namespace Joke.Server.Services.Joke;

internal class JokeSenderService(
    IHubContext<JokeHub> hubContext,
    ILogger<JokeSenderService> logger
) : IJokeSenderService
{
    private readonly IHubContext<JokeHub> _hubContext = hubContext ?? throw new ArgumentException(nameof(hubContext));
    private readonly ILogger<JokeSenderService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    public async Task SendJokeAsync(JokeEntity joke)
    {
        try
        {
            _logger.LogDebug("{@method} called", nameof(SendJokeAsync));
            
            await _hubContext.Clients.All.SendAsync("ReceiveJoke", joke);

            // TODO: Emit here
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error, failed to send jokes", nameof(SendJokeAsync));
            throw;
        }
    }
    public void EmitFetchedJokes()
    {
        // TODO: For UI
    }
    public void EmitSentJoke()
    {
        // TODO: For UI
    }
    public void EmitReceivedJoke()
    {
        // TODO: For UI
    }
}