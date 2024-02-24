using BusinessLogic.EventArgs;
using BusinessLogic.PlayerData;

namespace BusinessLogic;

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

    public void OnTeamUpdated(TeamUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }
    public void AttachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        Updated += observer;
    }
    public void DetachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        Updated -= observer;
    }
    public void AttachObserverToAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.AttachObserver(observer);
    }
    public void DetachObserverFromAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.DetachObserver(observer);
    }
    
    public void PlayerChangedHandler(object? sender, PlayerUpdatedEventArgs e)
    {
        CardsCount = GetBadCardsCount();
    }

    private int GetBadCardsCount()
        => Players.Sum(player => player.GetBadCardsCount());
}