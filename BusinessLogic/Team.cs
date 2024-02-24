using BusinessLogic.EventArgs;
using BusinessLogic.PlayerData;

namespace BusinessLogic;

/// <summary>
/// Represents a football team.
/// </summary>
public class Team
{
    public List<Player> Players { get; set; }
    public string Name { get; set; }

    private int _cardsCount;
    public int CardsCount
    {
        get => _cardsCount;
        set
        {
            _cardsCount = value;
            OnTeamUpdated(new TeamUpdatedEventArgs(DateTime.Now, this, _cardsCount));
        }
    }

    private EventHandler<TeamUpdatedEventArgs>? Updated;
    
    public Team(string name, List<Player> players)
    {
        Name = name;
        Players = players;
        CardsCount = GetBadCardsCount();
    }

    /// <summary>
    /// Raises the TeamUpdated event.
    /// </summary>
    /// <param name="e">The event arguments containing the update information.</param>
    public void OnTeamUpdated(TeamUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }
    
    /// <summary>
    /// Attaches an observer to the TeamUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        Updated += observer;
    }
    
    /// <summary>
    /// Detaches an observer from the TeamUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        Updated -= observer;
    }
    
    /// <summary>
    /// Attaches an observer to the PlayerUpdated event of all players in the team.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachObserverToAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.AttachObserver(observer);
    }
    
    /// <summary>
    /// Detaches an observer from the PlayerUpdated event of all players in the team.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachObserverFromAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.DetachObserver(observer);
    }
    
    /// <summary>
    /// Handler method for the PlayerUpdated event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments containing the update information.</param>
    public void PlayerChangedHandler(object? sender, PlayerUpdatedEventArgs e)
    {
        CardsCount = GetBadCardsCount();
    }

    private int GetBadCardsCount()
        => Players.Sum(player => player.GetBadCardsCount());
}