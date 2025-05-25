using Joke.Server.Hubs;
using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Models.Joke;
using Microsoft.AspNetCore.SignalR;

namespace Joke.Server.Services.Joke;

/// <inheritdoc cref="IJokeSenderService" />
internal class JokeSenderService(
    IHubContext<JokeHub> hubContext,
    IJokeViewService jokeViewService,
    ILogger<JokeSenderService> logger
) : IJokeSenderService
{
    private readonly IHubContext<JokeHub> _hubContext = hubContext ?? throw new ArgumentException(nameof(hubContext));
    private readonly IJokeViewService _jokeViewService = jokeViewService ?? throw new ArgumentException(nameof(jokeViewService));
    private readonly ILogger<JokeSenderService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    public async Task SendJokeAsync(JokeEntity joke)
    {
        try
        {
            _logger.LogDebug("{@method} called", nameof(SendJokeAsync));
            
            await _hubContext.Clients.All.SendAsync("ReceiveJoke", joke);

            _jokeViewService.EmitSentJokesChange();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error, failed to send jokes", nameof(SendJokeAsync));
            throw;
        }
    }
    public void EmitFetchedJokes() => _jokeViewService.EmitJokesChange();
    public void EmitSentJoke() => _jokeViewService.EmitSentJokesChange();
    public void EmitReceivedJoke() => _jokeViewService.EmitTranslatedJokesChange();
}