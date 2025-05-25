using Joke.Data.Interfaces.Repositories.Joke;
using Joke.Server.Interfaces.Services.Joke;
using Joke.Shared.Constants;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Response;
using Joke.Shared.PromptRequests;

namespace Joke.Server.Services.Joke;

internal class JokeService(
    IJokeSenderService jokeSenderService,
    IJokeRepository jokeRepository,
    IOpenRouterService openRouterService,
    ILogger<JokeService> logger
) : IJokeService
{
    private readonly IJokeSenderService _jokeSenderService = jokeSenderService ?? throw new ArgumentException(nameof(jokeSenderService));
    private readonly IJokeRepository _jokeRepository = jokeRepository ?? throw new ArgumentException(nameof(jokeRepository));
    private readonly IOpenRouterService _openRouterService = openRouterService ?? throw new ArgumentException(nameof(openRouterService));
    private readonly ILogger<JokeService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    private readonly SemaphoreSlim _semaphore = new(Limits.ServerConcurrentOperationThreadLimit);
    
    public async Task RunSenderTaskAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} RunSenderTask loop started", nameof(RunSenderTaskAsync));
            while (!cancellationToken.IsCancellationRequested)
            {
                // To avoid continually fetching new jokes, await the client
                // processing confirmation. This will generally happen when a message
                // is returned to the server and removed from the SentJokes collection.
                if (_jokeRepository.GetSentJokesCount() >= Limits.ClientIntakeLimit)
                {
                    _logger.LogInformation("{@method} Awaiting translations for sent jokes...", nameof(RunSenderTaskAsync));
                    await Task.Delay(Limits.ServerSenderDelay, cancellationToken);
                    continue;
                }
                
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

    public async Task ReceiveJokeAsync(JokeEntity? receivedJoke, bool isSuccess = true)
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

            if (isSuccess)
            {
                _jokeRepository.AddTranslatedJokes([receivedJoke]);
            }
            else
            {
                _jokeRepository.RemoveSentJokes([receivedJoke]);
            }
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
            await _semaphore.WaitAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            
            var request = JokesRequest.GetRequest(10);
            var response = await _openRouterService.QueryModelAsync<JokesResponse>(request);
            if (response is null)
            {
                _logger.LogWarning("{@method} Warning - fetched jokes object is null", nameof(FetchJokesAsync));
                return;
            }
            
            _jokeRepository.AddJokes(response?.Jokes ?? []);
            _jokeSenderService.EmitFetchedJokes();
        }
        catch (OperationCanceledException ex) 
        {
            _logger.LogInformation(ex, "{@method} Fetch operation cancelled", nameof(FetchJokesAsync));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} Error - failed to fetch new jokes", nameof(FetchJokesAsync));
        }
        finally
        {
            _semaphore.Release();
            _logger.LogDebug("{@method} Permit released, {@permits} semaphore permits available", 
                nameof(FetchJokesAsync), _semaphore.CurrentCount);
        }
    }

    public async Task SendJokeAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} Called", nameof(SendJokeAsync));
            cancellationToken.ThrowIfCancellationRequested();
            
            var newJoke = _jokeRepository.GetJoke();
            if (newJoke is null)
            {
                _logger.LogInformation("{@method} No jokes available, fetching new jokes", nameof(SendJokeAsync));
                await FetchJokesAsync(cancellationToken);
                return;
            }
        
            _jokeRepository.MarkSentJokes([newJoke.Id]);
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