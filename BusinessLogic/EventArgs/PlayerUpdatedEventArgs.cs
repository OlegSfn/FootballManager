namespace BusinessLogic.EventArgs;

public class PlayerUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime { get;}
    public List<Stat> Stats { get; }
    
    
    public PlayerUpdatedEventArgs(DateTime updateTime, List<Stat> stats)
    {
        UpdateTime = updateTime;
        Stats = stats;
    }
}