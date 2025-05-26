using Joke.Shared.Constants;
using Joke.Shared.Extensions;
using Joke.Shared.Interfaces.Services.Http;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Mock;
using Joke.Shared.Models.Prompt.Request;
using Joke.Shared.Models.Prompt.Response;
using Joke.Shared.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Joke.Shared.Services.OpenRouter;

/// <inheritdoc cref="IOpenRouterService" />
public class OpenRouterService(
    IHttpService httpService,
    IOptions<OpenRouterOptions> options,
    ILogger<OpenRouterService> logger
) : IOpenRouterService
{
    private readonly IHttpService _httpService = httpService ?? throw new ArgumentException(nameof(httpService));
    private readonly OpenRouterOptions _options = options.Value ?? throw new ArgumentException(nameof(options));
    private readonly ILogger<OpenRouterService> _logger = logger ?? throw new ArgumentException(nameof(logger));
    
    public async Task<T?> QueryModelAsync<T>(PromptRequest request)
    {
        try
        {
            var responseAsString = _options.IsTestMode 
                ? MockResponse.GetData<T>(request.Id)
                : await QueryModelAsync(request);

            if (responseAsString.IsNullOrEmpty())
            {
                _logger.LogWarning("{@method} response string is null or empty", nameof(QueryModelAsync));
                return default;
            }
            
            // For test mode, add response delay to reflect API conditions
            if (_options is { IsTestMode: true, TestDelay: > 0 })
            {
                await Task.Delay(_options.TestDelay);
            }
            
            // Get full OpenRouter response (including metadata) then deserialize into 
            // relevant entity(s)
            var responseAsFullObject = responseAsString.DeserializeT<PromptResponse>(JsonOptions.DefaultSerializerOptions);
            if (responseAsFullObject is null)
            {
                _logger.LogWarning("{@method} response object could not be deserialized", nameof(QueryModelAsync));
                return default;
            }

            // Should always have a single choice however, OpenRouter responses support multi-choice
            // for certain models
            var responseChoice = responseAsFullObject.Choices.FirstOrDefault();
            if (responseChoice is null)
            {
                _logger.LogWarning("{@method} no response choice available not be deserialized", nameof(QueryModelAsync));
                return default;
            }
            
            // Get the final output object using response entity choice content
            var responseAsOutputObject = responseChoice!
                .Message.Content
                .SanitiseFromMarkdown()
                .DeserializeT<T>(JsonOptions.DefaultSerializerOptions);
            
            if (responseAsOutputObject is not null)
            {
                return responseAsOutputObject;
            }
            
            logger.LogWarning("{@method} desired output object could not be deserialized", nameof(QueryModelAsync));
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error, failed to query OpenRouter", nameof(QueryModelAsync));
            throw;
        }
    }
    
    private async Task<string> QueryModelAsync(PromptRequest request)
    {
        _logger.LogDebug("{@method} called", nameof(QueryModelAsync));
        _logger.LogTrace(" with request {@request}", request.SerializeT());
        
        try
        {
            // Set query model
            request.Model = _options.QueryModel.GetOpenRouterModel().GetDescription();
            
            var jsonPayload = request.SerializeT(JsonOptions.DefaultSerializerOptions);
            var responseString = await _httpService.PostAsync(_options.Endpoint, jsonPayload, GetDefaultHeaders());
            if (responseString.IsJson())
            {
                _logger.LogDebug("{@method} success - response is valid JSON", nameof(QueryModelAsync));
            }
            else
            {
                _logger.LogWarning("{@method} warning - response is not valid JSON", nameof(QueryModelAsync));
                _logger.LogDebug("{@method} Response: {@responseString}", nameof(QueryModelAsync), responseString);
            }
            
            return responseString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error, failed to query OpenRouter", nameof(QueryModelAsync));
            throw;
        }
    }
    
    private Dictionary<string, string> GetDefaultHeaders()
    {
        return new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {_options.ApiKey}" },
            { "HTTP-Referer", "http://localhost" },
            { "X-Title", "Query LLM via OpenRouter" }
        };
    }
}