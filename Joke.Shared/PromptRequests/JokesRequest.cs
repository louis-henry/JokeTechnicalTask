using Joke.Shared.Constants;
using Joke.Shared.Extensions;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Request;
using Joke.Shared.Models.Prompt.Response;
using Joke.Shared.Types.OpenRouter;

namespace Joke.Shared.PromptRequests;

public static class JokesRequest
{
    public static PromptRequest GetRequest(int jokeCount)
    {
        var exampleSchema = new JokesResponse
        {
            Jokes = [
                new JokeEntity
                {
                    Id = Guid.NewGuid(),
                    Joke = "Why did the coffee file a police report?",
                    Answer = "It got mugged!"
                }
            ]
        };
        
        return new PromptRequest
        {
            Id = nameof(JokesRequest),
            Model = OpenRouterModelType.Default.GetDescription(),
            Messages = new List<PromptRequestMessage>
            {
                new PromptRequestMessage
                {
                    Role = "system",
                    Content = "You are a joke-telling assistant.\n\n" +
                              "- Always respond using the specified JSON schema.\n" +
                              "- Do not use any markdown characters.\n" +
                              "- The property 'id' must be a valid GUID.\n" +
                              "- Responses should be valid JSON with a single root object named 'jokes'.\n\n" +
                              "Adhere strictly to the following JSON schema for your response:\n" +
                              exampleSchema.SerializeT(JsonOptions.DefaultSerializerOptions)
                },
                new PromptRequestMessage
                {
                    Role = "user",
                    Content = $"Tell me {jokeCount} jokes."
                }
            }
        };
    }
}