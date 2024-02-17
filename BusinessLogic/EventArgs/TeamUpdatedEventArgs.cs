namespace BusinessLogic.EventArgs;

public class TeamUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime {get; }
    public int NewCardsCount { get; }
}