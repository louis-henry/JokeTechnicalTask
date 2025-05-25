using Joke.Client.Interfaces.Services.Joke;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Models.Joke;
using Joke.Shared.PromptRequests;
using Microsoft.Extensions.Logging;

namespace Joke.Client.Services.Joke;

/// <inheritdoc cref="IJokeService" />
internal class JokeService(
    IOpenRouterService openRouterService,
    ILogger<JokeService> logger
) : IJokeService
{
    private readonly IOpenRouterService _openRouterService = openRouterService ?? throw new ArgumentException(nameof(openRouterService));
    private readonly ILogger<JokeService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    
    public async Task<JokeEntity?> TranslateJokeAsync(JokeEntity joke, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} Called with Id: {@id}", nameof(TranslateJokeAsync), joke.Id);
            
            var request = JokesTranslateRequest.GetRequest(joke);
            var responseJoke = await _openRouterService.QueryModelAsync<JokeEntity>(request);
            
            if (responseJoke is null)
            {
                _logger.LogWarning("{@method} Translated joke returned null", nameof(TranslateJokeAsync));
            }
            
            return responseJoke;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} Error - failed to translate jokes", nameof(TranslateJokeAsync));
            throw;
        }
    }
}