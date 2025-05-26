using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Models.Communication;
using Joke.Shared.Models.Joke;
using Joke.Shared.Types.Connection;
using Microsoft.AspNetCore.SignalR;

namespace Joke.Server.Hubs;

public class JokeHub(
    IJokeService jokeService,
    IJokeViewService jokeViewService,
    ILogger<JokeHub> logger
) : Hub
{
    private readonly IJokeService _jokeService = jokeService ?? throw new ArgumentException(nameof(jokeService));
    private readonly IJokeViewService _jokeViewService = jokeViewService ?? throw new ArgumentException(nameof(jokeViewService));
    private readonly ILogger<JokeHub> _logger = logger ?? throw new ArgumentException(nameof(logger));
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("{@method} Client connected: {ConnectionId}", nameof(OnConnectedAsync), Context.ConnectionId);
        _jokeViewService.EmitConnectionStatusChangeChange(ConnectionStatus.Connected);
        
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, Context.ConnectionAborted);
        _ = _jokeService.RunSenderTaskAsync(linkedCts.Token);
        
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug("{@method} Client disconnected: {ConnectionId}", nameof(OnDisconnectedAsync), Context.ConnectionId);
        _jokeViewService.EmitConnectionStatusChangeChange(ConnectionStatus.Disconnected);
        
        await _cancellationTokenSource.CancelAsync();
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task ReturnJoke(ClientProcessedResult<JokeEntity> result)
    {
        _logger.LogInformation("{@method} Received from Client: {ConnectionId} with result Id: {@id}", nameof(ReturnJoke), Context.ConnectionId, result.Processed?.Id);
        await _jokeService.ReceiveJokeAsync(result.IsSuccess ? result.Processed : result.Original, result.IsSuccess);
    }
}