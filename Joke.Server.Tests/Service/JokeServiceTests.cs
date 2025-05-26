using Joke.Data.Interfaces.Repositories.Joke;
using Joke.Server.Interfaces.Services.Joke;
using Joke.Server.Services.Joke;
using Joke.Shared.Interfaces.Services.OpenRouter;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Request;
using Joke.Shared.Models.Prompt.Response;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Joke.Server.Tests.Service;

public class JokeServiceTests
{
    private readonly Mock<IJokeSenderService> _jokeSenderServiceMock; 
    private readonly Mock<IJokeRepository> _jokeRepositoryMock; 
    private readonly Mock<IOpenRouterService> _openRouterServiceMock;
    private readonly JokeService _jokeService;  
    private readonly JokeEntity _jokeTestEntity;  
      
    public JokeServiceTests()  
    {  
        _jokeSenderServiceMock = new Mock<IJokeSenderService>();
        _jokeRepositoryMock = new Mock<IJokeRepository>();
        _openRouterServiceMock = new Mock<IOpenRouterService>();
        _jokeService = new JokeService(
            _jokeSenderServiceMock.Object,
            _jokeRepositoryMock.Object,
            _openRouterServiceMock.Object,
            NullLogger<JokeService>.Instance);  
  
        _jokeTestEntity = new JokeEntity  
        {  
            Id = Guid.NewGuid(),  
            Joke = "Placeholder Joke Value",  
            Answer = "Placeholder Answer Value"  
        };  
    }  
  
    [Fact]  
    public async Task SendJokeAsync_WhenNoJokes_ReturnsTrue()  
    {  
        // Arrange
        _jokeRepositoryMock  
            .Setup(service => service.GetJoke())  
            .Returns((JokeEntity?)null); 
        _openRouterServiceMock  
            .Setup(service => service.QueryModelAsync<JokesResponse>(It.IsAny<PromptRequest>()))  
            .ReturnsAsync(new JokesResponse { Jokes = [_jokeTestEntity] }); 
        
        // Act
        await _jokeService.SendJokeAsync(CancellationToken.None); 
        
        // Assert
        _jokeRepositoryMock.Verify(s => s.AddJokes(It.IsAny<IEnumerable<JokeEntity>>()), Times.AtLeastOnce());
        _jokeSenderServiceMock.Verify(s => s.EmitFetchedJokes(), Times.AtLeastOnce());
    }
    
    [Fact]  
    public async Task SendJokeAsync_WhenJokesAvailable_ReturnsTrue()  
    {
        // Arrange
        _jokeRepositoryMock  
            .Setup(service => service.GetJoke())  
            .Returns(_jokeTestEntity); 
        
        // Act
        await _jokeService.SendJokeAsync(CancellationToken.None);
        
        // Assert
        _jokeRepositoryMock.Verify(s => s.MarkSentJokes(It.IsAny<IEnumerable<Guid>>()), Times.AtLeastOnce());
        _jokeSenderServiceMock.Verify(s => s.SendJokeAsync(_jokeTestEntity), Times.AtLeastOnce());
    }
    
    // Add more tests here...
}