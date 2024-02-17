using BusinessLogic.EventArgs;

namespace BusinessLogic;

public class GameData
{
    private List<Player> Players { get; set; }
    private List<Team> Teams { get; set; } = null!;

    public GameData(List<Player> players, List<Team> teams)
    {
        Players = players;
        Teams = teams;
    }

    public GameData(List<Player> players)
    {
        Players = players;
        CreateTeams();
    }

    public void OnGameUpdated(GameUpdatedEventArgs e)
    {
        throw new NotImplementedException();
    }
    public void AttachObserver(EventHandler<GameUpdatedEventArgs> observer)
    {
        throw new NotImplementedException();
    }
    public void DetachObserver(EventHandler<GameUpdatedEventArgs> observer)
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
    public void TeamChangedHandler()
    {
        throw new NotImplementedException();
    }

    public Player[] SortPlayers()    
    {
        throw new NotImplementedException();
    }

    // O(nm) - creation of teams, n - number of players, m - number of teams.
    private void CreateTeams()
    {
        HashSet<string> teamNames = new HashSet<string>();
        // Find unique teams.
        foreach (var player in Players)
            teamNames.Add(player.Team);
        
        // Create teams.
        foreach (var name in teamNames)
            Teams.Add(new Team(name, new List<Player>()));
        
        // Add players in teams;
        foreach (var player in Players)
            foreach (var team in Teams)
            if (player.Team == team.Name)
                team.Players.Add(player);
    }
}