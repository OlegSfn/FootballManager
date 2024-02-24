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
        set
        {
            _cardsCount = value;
            OnTeamUpdated(new TeamUpdatedEventArgs(this, _cardsCount));
        }
    }

    private EventHandler<TeamUpdatedEventArgs>? _teamUpdated;
    
    public Team(string name, List<Player> players)
    {
        Name = name;
        Players = players;
        CardsCount = GetBadCardsCount();
    }
    
    public Team() : this("", new List<Player>()) {}
    
    /// <summary>
    /// Attaches an observer to the TeamUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        _teamUpdated += observer;
    }
    
    /// <summary>
    /// Detaches an observer from the TeamUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        _teamUpdated -= observer;
    }
    
    /// <summary>
    /// Handler method for the PlayerUpdated event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments containing the update information.</param>
    public void PlayerChangedHandler(object? sender, PlayerReceivedStats e)
    {
        CardsCount = GetBadCardsCount();
    }

    private void OnTeamUpdated(TeamUpdatedEventArgs e)
    {
        var temp = _teamUpdated;
        temp?.Invoke(this, e);
    }
    
    private int GetBadCardsCount()
        => Players.Sum(player => player.GetBadCardsCount());
}