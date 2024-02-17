namespace BusinessLogic.EventArgs;

public class GameUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime {get; }
    public string Message { get; }
    
    public GameUpdatedEventArgs(DateTime updateTime, string message)
    {
        UpdateTime = updateTime;
        Message = message;
    }
}