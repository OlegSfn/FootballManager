using System.Collections;
using System.Text.Json;
using BusinessLogic.EventArgs;

namespace BusinessLogic;

public class GameData
{
    public List<Player> Players { get; }
    public List<Team> Teams { get; }
    
    public string FileName { get; }

    public string[] Positions { get; } = {"Midfielder", "Goalkeeper", "Defender", "Forward" };
    
    private EventHandler<GameUpdatedEventArgs>? Updated;

    public GameData(List<Player> players, List<Team> teams, string fileName)
    {
        Players = players;
        Teams = teams;
        FileName = fileName;
    }

    public GameData(List<Player> players, string fileName)
    {
        Players = players;
        FileName = fileName;
        Teams = new List<Team>();
        CreateTeams();
    }

    public override string ToString() => FileName;
    
    public void OnGameUpdated(GameUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }
    public void AttachObserver(EventHandler<GameUpdatedEventArgs> observer)
    {
        Updated += observer;
    }
    public void DetachObserver(EventHandler<GameUpdatedEventArgs> observer)
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

    public Player[] SortPlayers(string fieldName, bool isReversed)
    {
        var sortedPlayers = new Player[Players.Count];
        for (int i = 0; i < Players.Count; i++)
        {
            var curPlayer = Players[i];
            sortedPlayers[i] = new Player(curPlayer.Id, curPlayer.Name, curPlayer.Position, curPlayer.JerseyNumber, 
                curPlayer.TeamName, new List<Stat>(curPlayer.Stats != null ? curPlayer.Stats : new List<Stat>()));
        }
        
        var mult = isReversed ? -1 : 1;
        Array.Sort(sortedPlayers, Comparer<Player>.Create((player1, player2) => 
            player1.CompareTo(player2, fieldName) * mult));

        return sortedPlayers;
    }

    // TODO: Assert fileName
    public static void SaveToFile(string fileName, List<Player> players)
    {
        using StreamWriter sw = new($"{fileName}");
        var options = new JsonSerializerOptions { WriteIndented = true };
        sw.Write(JsonSerializer.Serialize(players, options));
    }
    public void SaveToFile(string fileName)
    {
        using StreamWriter sw = new($"{fileName}");
        var options = new JsonSerializerOptions { WriteIndented = true };
        sw.Write(JsonSerializer.Serialize(Players, options));
    }
    

    // O(nm) - creation of teams, n - number of players, m - number of teams.
    private void CreateTeams()
    {
        HashSet<string> teamNames = new HashSet<string>();
        // Find unique teams.
        foreach (var player in Players)
            teamNames.Add(player.TeamName);
        
        // Create teams.
        foreach (var name in teamNames)
            CreateNewTeam(name);
        
        // Add players in teams;
        foreach (var player in Players)
            foreach (var team in Teams)
                if (player.TeamName == team.Name)
                {
                    team.Players.Add(player);
                    player.AttachObserver(team.PlayerChangedHandler);
                    player.Team = team;
                }
    }

    public void CreateNewTeam(string name, List<Player> players)
    {
        Team team = new Team(name, players);
        Teams.Add(team);
        team.AttachObserver(TeamChangedHandler);
    }

    public void CreateNewTeam(string name)
        => CreateNewTeam(name, new List<Player>()); 
    
    private void TeamChangedHandler(object? sender, TeamUpdatedEventArgs e)
    {
        if (e.NewCardsCount > 7)
            OnGameUpdated(new GameUpdatedEventArgs(DateTime.Now, $"Team {e.Team.Name} was disqualified."));
    }
}