namespace Joke.Client.Interfaces.Services.Menu;

/// <summary>
/// Represents a service responsible for displaying and managing the application menu.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Runs the interactive menu flow, handling user input and triggering related operations.
    /// </summary>
    /// <returns>A task that represents the asynchronous execution of the menu logic.</returns>
    Task RunAsync();
}