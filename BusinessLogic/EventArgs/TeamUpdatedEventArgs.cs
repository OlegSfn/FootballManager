namespace BusinessLogic.EventArgs;

/// <summary>
/// Provides data for the event that occurs when a team is updated.
/// </summary>
public class TeamUpdatedEventArgs : System.EventArgs
{
    public int NewCardsCount { get; }
    public Team Team { get; }

    public TeamUpdatedEventArgs(Team team, int newCardsCount)
    {
        Team = team;
        NewCardsCount = newCardsCount;
    }
    
    public TeamUpdatedEventArgs() : this(new Team(), 0) {}
}