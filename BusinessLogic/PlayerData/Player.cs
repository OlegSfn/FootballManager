using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using BusinessLogic.EventArgs;

namespace BusinessLogic.PlayerData;

/// <summary>
/// Represents a player in a football team.
/// </summary>
public class Player
{
    [JsonPropertyName("playerId")] public string Id { get; }

    private string _name;
    [JsonPropertyName("playerName")]
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
                return;
            
            _name = value;
            OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
        }
    }

    private string _position;

    [JsonPropertyName("position")]
    public string Position
    {
        get => _position;
        set
        {
            if (_position == value)
                return;

            _position = value;
            OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
        }
    }

    private int _jerseyNumber;

    [JsonPropertyName("jerseyNumber")]
    public int JerseyNumber
    {
        get => _jerseyNumber;
        set
        {
            if (value < 0)
                throw new ArgumentException("Jersey number cannot be negative");
            
            if (_jerseyNumber == value)
                return;

            _jerseyNumber = value;
            OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
        }
    }

    private string _teamNameName;

    [JsonPropertyName("team")]
    public string TeamName
    {
        get => _teamNameName;
        set
        {
            if (_teamNameName == value)
                return;
            
            _teamNameName = value;
            OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
        }
    }

    [JsonIgnore]
    public Team Team { get; set; }

    private readonly List<Stat> _stats;

    [JsonPropertyName("stats")]
    public List<Stat> Stats
    {
        get => _stats;
        init
        {
            _stats = value;
            OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
        }
    }

    private EventHandler<PlayerUpdatedEventArgs>? Updated;
    private EventHandler<PlayerReceivedStats>? _statsReceived;
    
    [JsonConstructor]
    public Player(string id, string name, string position, int jerseyNumber, string teamName, List<Stat> stats)
    {
        Id = id;
        Name = name;
        Position = position;
        JerseyNumber = jerseyNumber;
        TeamName = teamName;
        Stats = stats;
    }
    
    public Player() : this("", "","", 0, "", new List<Stat>()){}

    /// <summary>
    /// Compares the current player with another player based on the specified field name.
    /// </summary>
    /// <param name="other">The other player to compare with.</param>
    /// <param name="fieldName">The name of the field to compare.</param>
    /// <returns>An integer that indicates the relative order of the players.</returns>
    public int CompareTo(Player other, string fieldName)
        => fieldName switch
            {
                "Id" => Comparer.DefaultInvariant.Compare(Id, other.Id),
                "Name" or "Имя" => Comparer.DefaultInvariant.Compare(Name, other.Name),
                "Position" or "Позиция" => Comparer.DefaultInvariant.Compare(Position, other.Position),
                "Jersey number" or "Игровой номер" => Comparer.DefaultInvariant.Compare(JerseyNumber, other.JerseyNumber),
                "Team" or "Команда" => Comparer.DefaultInvariant.Compare(TeamName, other.TeamName),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldName), fieldName)
            };
    
    /// <summary>
    /// Serializes the player object to its JSON representation.
    /// </summary>
    /// <returns>A JSON string representing the player object.</returns>
    public string ToJson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(this, options);
    }

    /// <summary>
    /// Attaches an observer to the PlayerUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachUpdatedObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        Updated += observer;
    }

    /// <summary>
    /// Detaches an observer from the PlayerUpdated event.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachUpdatedObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        Updated -= observer;
    }
    
    /// <summary>
    /// Raises the PlayerUpdated event.
    /// </summary>
    /// <param name="e">The event arguments containing the update information.</param>
    public void OnPlayerUpdated(PlayerUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }

    /// <summary>
    /// Attaches an observer to the PlayerReceivedStats event.
    /// </summary>
    /// <param name="observer">The event handler to attach.</param>
    public void AttachStatsReceivedObserver(EventHandler<PlayerReceivedStats> observer)
    {
        _statsReceived += observer;
    }

    /// <summary>
    /// Detaches an observer from the PlayerReceivedStats event.
    /// </summary>
    /// <param name="observer">The event handler to detach.</param>
    public void DetachUpdatedObserver(EventHandler<PlayerReceivedStats> observer)
    {
        _statsReceived -= observer;
    }
    
    /// <summary>
    /// Raises the PlayerReceivedStats event.
    /// </summary>
    /// <param name="e">The event arguments containing the update information.</param>
    public void OnPlayerStatsReceived(PlayerReceivedStats e)
    {
        var temp = _statsReceived;
        temp?.Invoke(this, e);
    }

    /// <summary>
    /// Gets the count of bad cards associated with the player.
    /// </summary>
    /// <returns>The count of bad cards.</returns>
    public int GetBadCardsCount()
        => Stats.Count(stat => stat.GetEnumType() is StatType.RedCards or StatType.YellowCards);
}