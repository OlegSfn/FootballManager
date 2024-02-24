namespace BusinessLogic.EventArgs;

/// <summary>
/// Provides data for the event that occurs when a player is updated.
/// </summary>
public class PlayerUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime { get;}
    
    public PlayerUpdatedEventArgs(DateTime updateTime)
    {
        UpdateTime = updateTime;
    }
}