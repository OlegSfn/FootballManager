using BusinessLogic.EventArgs;

namespace BusinessLogic;

public class Player
{
    public string Id { get; }
    public string Name { get; }
    public string Position { get; }
    public int JerseyNumber { get;}
    public string Team { get; }
    public List<Stat> Stats { get; }
    
    private EventHandler<PlayerUpdatedEventArgs>? Updated;
    
    public Player(string id, string name, string position, int jerseyNumber, string team, List<Stat> stats)
    {
        Id = id;
        Name = name;
        Position = position;
        JerseyNumber = jerseyNumber;
        Team = team;
        Stats = stats;
    }
    
    public string ToJson()
    {
        throw new NotImplementedException();
    }

    public void OnPlayerUpdated(PlayerUpdatedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void AttachObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }

    public void DetachObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }

    public int GetBadCardsCount()
        => Stats.Count(stat => stat.Type is StatType.RedCards or StatType.YellowCards);
}