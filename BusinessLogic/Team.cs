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
        throw new NotImplementedException();
    }
    public void AttachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }
    public void DetachObserver(EventHandler<TeamUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }
    public void AttachObserverToAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }
    public void DetachObserverFromAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }
    public void PlayerChangedHandler()    
    {
        throw new NotImplementedException();
    }

    private int GetBadCardsCount()
        => Players.Sum(player => player.GetBadCardsCount());
}