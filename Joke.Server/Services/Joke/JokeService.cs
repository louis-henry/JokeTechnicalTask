using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Constants;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Response;
using Joke.Shared.PromptRequests;

namespace Joke.Server.Services.Joke;

internal class JokeService(
    IJokeSenderService jokeSenderService,
    IOpenRouterService openRouterService,
    ILogger<JokeService> logger
) : IJokeService
{
    private readonly IJokeSenderService _jokeSenderService = jokeSenderService ?? throw new ArgumentException(nameof(jokeSenderService));
    private readonly IOpenRouterService _openRouterService = openRouterService ?? throw new ArgumentException(nameof(openRouterService));
    private readonly ILogger<JokeService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    public async Task RunSenderTaskAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} RunSenderTask loop started", nameof(RunSenderTaskAsync));
            while (!cancellationToken.IsCancellationRequested)
            {
                await SendJokeAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                
                await Task.Delay(Limits.ServerSenderInterval, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("{@method} CancellationToken is cancelled", nameof(RunSenderTaskAsync));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} Error - unable to execute task", nameof(RunSenderTaskAsync));
        }
        _logger.LogInformation("{@method} Task complete", nameof(RunSenderTaskAsync));
    }

    public async Task ReceiveJokeAsync(JokeEntity? receivedJoke)
    {
        try
        {
            _logger.LogDebug("{@method} called", nameof(ReceiveJokeAsync));
            
            await Task.Yield();
            if (receivedJoke is null)
            {
                _logger.LogWarning("{@method} no translated jokes received", nameof(ReceiveJokeAsync));
                return;
            }
            
            // TODO: update when repo implemented
            _jokeSenderService.EmitReceivedJoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error, failed to process received jokes", nameof(ReceiveJokeAsync));
        }
    }

    public async Task FetchJokesAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} Called", nameof(FetchJokesAsync));
            cancellationToken.ThrowIfCancellationRequested();
            
            var request = JokesRequest.GetRequest(10);
            var response = await _openRouterService.QueryModelAsync<JokesResponse>(request);
            
            // TODO: Handler with repo
        }
        catch (OperationCanceledException ex) 
        {
            _logger.LogInformation(ex, "{@method} Fetch operation cancelled", nameof(FetchJokesAsync));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} Error - failed to fetch new jokes", nameof(FetchJokesAsync));
        }
    }

    public async Task SendJokeAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} Called", nameof(SendJokeAsync));
            cancellationToken.ThrowIfCancellationRequested();
            
            // TODO: update when repo implemented
            var newJoke = new JokeEntity
            {
                Id = Guid.NewGuid(),
                Joke = "Placegolder"
            };
            
            if (newJoke is null)
            {
                _logger.LogInformation("{@method} No jokes available, fetching new jokes", nameof(SendJokeAsync));
                await FetchJokesAsync(cancellationToken);
                return;
            }
        
            // TODO: update repo
            await _jokeSenderService.SendJokeAsync(newJoke);
        }
        catch (OperationCanceledException ex) 
        {
            _logger.LogInformation(ex, "{@method} Fetch operation cancelled", nameof(SendJokeAsync));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} Error occurred executing background task.", nameof(SendJokeAsync));
        }
    }
}