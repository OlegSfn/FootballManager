namespace BusinessLogic.EventArgs;

/// <summary>
/// Provides data for the event that occurs when a team is updated.
/// </summary>
public class TeamUpdatedEventArgs : System.EventArgs
{
    public DateTime UpdateTime {get; }
    public int NewCardsCount { get; }
    public Team Team { get; }

    public TeamUpdatedEventArgs(DateTime updateTime, Team team, int newCardsCount)
    {
        UpdateTime = updateTime;
        Team = team;
        NewCardsCount = newCardsCount;
    }
}