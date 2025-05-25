using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Models.Communication;
using Joke.Shared.Models.Joke;
using Microsoft.AspNetCore.SignalR;

namespace Joke.Server.Hubs;

public class JokeHub(
    IJokeService jokeService,
    ILogger<JokeHub> logger
) : Hub
{
    private readonly IJokeService _jokeService = jokeService ?? throw new ArgumentException(nameof(jokeService));
    private readonly ILogger<JokeHub> _logger = logger ?? throw new ArgumentException(nameof(logger));
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected: {ConnectionId}", Context.ConnectionId);
        
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, Context.ConnectionAborted);
        _ = _jokeService.RunSenderTaskAsync(linkedCts.Token);
        
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug("Client disconnected: {ConnectionId}", Context.ConnectionId);
        
        await _cancellationTokenSource.CancelAsync();
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task ReturnJoke(ClientProcessedResult<JokeEntity> result)
    {
        _logger.LogInformation("Received from Client: {ConnectionId} with result Id: {@id}", Context.ConnectionId, result.Processed?.Id);
        await _jokeService.ReceiveJokeAsync(result.Processed);
    }
}