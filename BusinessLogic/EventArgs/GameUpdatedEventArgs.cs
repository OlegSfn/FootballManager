namespace BusinessLogic.EventArgs;

public class GameUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime {get; }
    public string Message { get; }
}