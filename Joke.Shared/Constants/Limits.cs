namespace Joke.Shared.Constants;

public static class Limits
{
    // Server
    public const int ServerConcurrentOperationThreadLimit = 1;
    public const int ServerSenderDelay = 1000;
    public const int ServerSenderInterval = 200;
    
    // Client
    public const int ClientConcurrentOperationThreadLimit = 2;
    public const int ClientEndCount = 5;
    public const int ClientIntakeLimit = 5;
}