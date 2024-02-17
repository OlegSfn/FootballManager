namespace BusinessLogic.EventArgs;

public class PlayerUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime { get;}
    
    public PlayerUpdatedEventArgs(DateTime updateTime)
    {
        UpdateTime = updateTime;
    }
}