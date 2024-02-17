namespace BusinessLogic.EventArgs;

public class PlayerUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime { get;}
    public List<Stat> Stats { get; }
}