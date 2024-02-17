using BusinessLogic.EventArgs;

namespace BusinessLogic;

public class Team
{
    public List<Player> Players { get; set; }
    public string Name { get; set; }
    public int CardsCount { get; set; }
    private EventHandler<TeamUpdatedEventArgs>? Updated;
    
    public Team(string name, List<Player> players)
    {
        Name = name;
        Players = players;
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
        throw new NotImplementedException();
    }

    private int GetBadCardsCount()
        => Players.Sum(player => player.GetBadCardsCount());
}