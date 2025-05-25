using System.Diagnostics;
using Joke.Shared.Options;
using Joke.Client.Interfaces.Handlers.Joke;
using Joke.Client.Interfaces.Services.Joke;
using Joke.Shared.Constants;
using Joke.Shared.Models.Communication;
using Joke.Shared.Models.Joke;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Joke.Client.Handlers.Joke;

internal class JokeHandler(
    IHostApplicationLifetime hostApplicationLifetime,
    IJokeService jokeService,
    IOptions<ServerOptions> serverOptions,
    ILogger<JokeHandler> logger
) : IJokeHandler
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentException(nameof(hostApplicationLifetime));
    private readonly IJokeService _jokeService = jokeService ?? throw new ArgumentException(nameof(jokeService)); 
    private readonly ILogger<JokeHandler> _logger = logger ?? throw new ArgumentException(nameof(logger));
    private readonly HubConnection _connection = new HubConnectionBuilder()
        .WithUrl($"{serverOptions?.Value?.FullUrl ?? string.Empty}")
        .WithAutomaticReconnect()
        .Build();
    
    private int _completeCount = 0;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    
    public async Task<bool> StartAsync()
    {
        _connection.On<JokeEntity>("ReceiveJoke", async message =>
        {
            await Task.Yield();
            Task.Run(() => ProcessMessageAsync(message, _cts.Token));
        });
        
        try
        {
            await _connection.StartAsync();
            _logger.LogDebug("{@method} Connected to server", nameof(StartAsync));
            return true;
        }
        catch
        {
            _logger.LogError(" {@method} Server unavailable.", nameof(StartAsync));
            return false;
        }
    }

    public async Task StopAsync(bool exitApp = true)
    {
        if (_connection.State != HubConnectionState.Connected)
        {
            _logger.LogDebug("{@method} Client is already disconnected from server", nameof(StartAsync));
            return;
        }
        
        await _connection.StopAsync();
        await _connection.DisposeAsync();
        _logger.LogInformation("{@method} Disconnecting from server...", nameof(StartAsync));
        
        if (!exitApp)
            return;
        
        _logger.LogInformation("{@method} Stopping application...", nameof(StartAsync));
        _hostApplicationLifetime.StopApplication();
    }

    public async Task ProcessMessageAsync(JokeEntity message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{@method} Message received by client with Id: {@id}", 
                nameof(ProcessMessageAsync), message.Id);
            cancellationToken.ThrowIfCancellationRequested();
            
            // Do not process any more requests when limit exceeded
            if (_completeCount >= Limits.ClientEndCount)
            {
                _logger.LogWarning("{@method} Max requests already processed, returning", nameof(ProcessMessageAsync));
                return;
            }

            // Track translation time
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Translate
            var translatedJoke = await _jokeService.TranslateJokeAsync(message, cancellationToken);
            if (translatedJoke is null)
            {
                _logger.LogWarning("{@method} Translation failed for {@id}", nameof(ProcessMessageAsync), message.Id);
                return;
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            stopWatch.Stop();
            translatedJoke.TimeToTranslate = stopWatch.ElapsedMilliseconds;
            var result = new ClientProcessedResult<JokeEntity>
            {
                Original = message,
                Processed = translatedJoke
            };
            
            await _connection.InvokeAsync("ReturnJoke", result, cancellationToken: cancellationToken);
            _logger.LogInformation("{@method} Message sent from client with Id: {@id}", "ReceiveJoke", message.Id);
            _completeCount++;
        }
        catch (OperationCanceledException) 
        {
            _logger.LogInformation("{@method} Message operation cancelled", nameof(ProcessMessageAsync));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@method} error - could not process message", nameof(ProcessMessageAsync));
        }
        finally
        {
            if (_completeCount >= Limits.ClientEndCount)
            {
                await StopAsync();
            }
        }
    }
}