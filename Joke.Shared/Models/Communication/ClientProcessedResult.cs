namespace Joke.Shared.Models.Communication;

public class ClientProcessedResult<T>
{
    public T? Original { get; set; }
    public T? Processed { get; set; }
}