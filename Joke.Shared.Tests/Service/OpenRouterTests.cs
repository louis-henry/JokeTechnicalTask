using Joke.Shared.Extensions;
using Joke.Shared.Interfaces.Services.Http;
using Joke.Shared.Models.Prompt.Response;
using Joke.Shared.Options;
using Joke.Shared.PromptRequests;
using Joke.Shared.Services.OpenRouter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Joke.Shared.Tests.Service;

public class OpenRouterTests
{
    private readonly Mock<IHttpService> _httpService;
    private readonly OpenRouterService _openRouterService;
    
    public OpenRouterTests()
    {
        var mockOptions = new Mock<IOptions<OpenRouterOptions>>();
        var mockLogger = new Mock<ILogger<OpenRouterService>>();
        mockOptions.Setup(o => o.Value).Returns(new OpenRouterOptions());
        
        _httpService = new Mock<IHttpService>();
        _openRouterService = new OpenRouterService(_httpService.Object, mockOptions.Object, mockLogger.Object);
    }
    
    [Fact]
    public async Task QueryModelAsync_WithTestMode_ReturnsTrue()
    {
        // Arrange
        var request = JokesRequest.GetRequest(10);
        
        // Act
        var result = await _openRouterService.QueryModelAsync<JokesResponse>(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Jokes.Count() == 10);
        Assert.All(result.Jokes, x =>
        {
            Assert.False(x.Id.ToString().IsNullOrEmpty());
            Assert.False(x.Joke?.ToString().IsNullOrEmpty());
            Assert.False(x.Answer?.ToString().IsNullOrEmpty());
        });
    }
    
    // Add more tests here...
}