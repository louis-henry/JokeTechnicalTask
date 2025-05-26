using Joke.Shared.Constants;
using Joke.Shared.Extensions;
using Joke.Shared.Models.Joke;
using Joke.Shared.Models.Prompt.Request;
using Joke.Shared.Types.OpenRouter;

namespace Joke.Shared.PromptRequests;

public static class JokesTranslateRequest
{
    public static PromptRequest GetRequest(JokeEntity entity)
    {
        return new PromptRequest
        {
            Id = entity.Id.ToString(),
            Model = OpenRouterModelType.Default.GetDescription(),
            Messages = new List<PromptRequestMessage>
            {
                new PromptRequestMessage
                {
                    Role = "system",
                    Content = "You are a joke-telling assistant.\n\n" +
                              "- Respond using the specified JSON schema.\n" +
                              "- Do not use any markdown characters.\n" +
                              "- The property 'id' must be a valid GUID.\n" +
                              "- The new translated joke property must be named 'translated_joke'.\n" +
                              "- The new translated answer property must be named 'translated_answer'.\n" +
                              "- The existing data/properties ('id', 'joke' and 'answer') must remain the same and also be returned.\n" +
                              "Adhere strictly to the following JSON schema for your response:\n" +
                              entity.SerializeT(JsonOptions.DefaultSerializerOptions)
                },
                new PromptRequestMessage
                {
                    Role = "user",
                    Content = "Translate the joke (from the provided schema) into standard German."
                }
            }
        };
    }
}