using System.Text.Json;
using BusinessLogic.EventArgs;
using BusinessLogic.PlayerData;

namespace BusinessLogic;

/// <summary>
/// Represents a football game.
/// </summary>
public class Game
{
    public List<Player> Players { get; }
    public List<Team> Teams { get; }
    
    public string FileName { get; }

    public string[] Positions { get; } = {"Midfielder", "Goalkeeper", "Defender", "Forward" };
    
    private EventHandler<GameUpdatedEventArgs>? Updated;

    public Game(List<Player> players, string fileName)
    {
        Players = players;
        FileName = fileName;
        Teams = new List<Team>();
        CreateTeams();
    }
    
    public Game() : this(new List<Player>(), "") {}

    public override string ToString() => FileName;
    
    /// <summary>
    /// Raises the GameUpdated event.
    /// </summary>
    /// <param name="e">The event arguments containing the update information.</param>
    public void OnGameUpdated(GameUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }
    
    /// <summary>
    /// Attaches an observer to the GameUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachObserver(EventHandler<GameUpdatedEventArgs> observer)
    {
        Updated += observer;
    }
    
    /// <summary>
    /// Detaches an observer from the GameUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachObserver(EventHandler<GameUpdatedEventArgs> observer)
    {
        Updated -= observer;
    }
    
    /// <summary>
    /// Attaches an observer to the PlayerUpdated event of all players in the game.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachObserverToAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.AttachObserver(observer);
    }
    
    /// <summary>
    /// Detaches an observer from the PlayerUpdated event of all players in the game.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachObserverFromAll(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        foreach (var player in Players)
            player.DetachObserver(observer);
    }

    /// <summary>
    /// Sorts the players based on the specified field name and sorting order.
    /// </summary>
    /// <param name="fieldName">The name of the field to sort by.</param>
    /// <param name="isReversed">A value indicating whether the sorting order is reversed.</param>
    /// <returns>The sorted array of players.</returns>
    public Player[] SortPlayers(string fieldName, bool isReversed)
    {
        var sortedPlayers = new Player[Players.Count];
        for (int i = 0; i < Players.Count; i++)
        {
            var curPlayer = Players[i];
            sortedPlayers[i] = new Player(curPlayer.Id, curPlayer.Name, curPlayer.Position, curPlayer.JerseyNumber, 
                curPlayer.TeamName, new List<Stat>(curPlayer.Stats));
        }
        
        var mult = isReversed ? -1 : 1;
        Array.Sort(sortedPlayers, Comparer<Player>.Create((player1, player2) => 
            player1.CompareTo(player2, fieldName) * mult));

        return sortedPlayers;
    }

    /// <summary>
    /// Saves the game data to a file.
    /// </summary>
    /// <param name="fileName">The name of the file to save to.</param>
    public void SaveToFile(string fileName)
    {
        using StreamWriter sw = new($"{fileName}");
        var options = new JsonSerializerOptions { WriteIndented = true };
        sw.Write(JsonSerializer.Serialize(Players, options));
    }
    

    /// <summary>
    /// Creates teams based on the players participating in the game.
    /// </summary>
    private void CreateTeams()
    {
        var teamNames = new HashSet<string>();
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

    /// <summary>
    /// Creates a new team with the specified name and list of players.
    /// </summary>
    /// <param name="name">The name of the team.</param>
    /// <param name="players">The list of players in the team.</param>
    public void CreateNewTeam(string name, List<Player> players)
    {
        var team = new Team(name, players);
        Teams.Add(team);
        team.AttachObserver(TeamChangedHandler);
    }

    /// <summary>
    /// Creates a new team with the specified name.
    /// </summary>
    /// <param name="name">The name of the team.</param>
    public void CreateNewTeam(string name)
        => CreateNewTeam(name, new List<Player>()); 
    
    private void TeamChangedHandler(object? sender, TeamUpdatedEventArgs e)
    {
        if (e.NewCardsCount > 7)
            OnGameUpdated(new GameUpdatedEventArgs($"Team {e.Team.Name} was disqualified."));
    }
}