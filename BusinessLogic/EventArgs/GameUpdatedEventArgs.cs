namespace BusinessLogic.EventArgs;

/// <summary>
/// Provides data for the event that occurs when a game is updated.
/// </summary>
public class GameUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime { get; }
    public string Message { get; }
    
    public GameUpdatedEventArgs(DateTime updateTime, string message)
    {
        UpdateTime = updateTime;
        Message = message;
    }
}