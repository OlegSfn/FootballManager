namespace BusinessLogic.EventArgs;

/// <summary>
/// Provides data for the event that occurs when a game is updated.
/// </summary>
public class GameUpdatedEventArgs : System.EventArgs
{
    public string Message { get; }
    
    public GameUpdatedEventArgs(string message)
    {
        Message = message;
    }
    
    public GameUpdatedEventArgs() : this("") { }
}