using System.Collections;
using BusinessLogic.EventArgs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessLogic;

public class Player
{
    [JsonPropertyName("playerId")] public string Id { get; }

    private string _name;
    [JsonPropertyName("playerName")]
    public string Name 
    { 
        get => _name;
        set => _name = value;
    }

    private string _position;

    [JsonPropertyName("position")]
    public string Position
    {
        get => _position;
        set => _position = value;
    }

    private int _jerseyNumber;

    [JsonPropertyName("jerseyNumber")]
    public int JerseyNumber
    {
        get => _jerseyNumber;
        set => _jerseyNumber = value;
    }

    private string _teamNameName;

    [JsonPropertyName("team")]
    public string TeamName
    {
        get => _teamNameName;
        set => _teamNameName = value;
    }
    
    [JsonIgnore]
    public Team Team { get; set; }

    private List<Stat> _stats;

    [JsonPropertyName("stats")]
    public List<Stat> Stats
    {
        get => _stats;
        set => _stats = value;
    }
    
    private EventHandler<PlayerUpdatedEventArgs>? Updated;
    
    public Player(string id, string name, string position, int jerseyNumber, string teamName, List<Stat> stats)
    {
        Id = id;
        Name = name;
        Position = position;
        JerseyNumber = jerseyNumber;
        TeamName = teamName;
        Stats = stats;
    }

    public int CompareTo(Player other, string fieldName)
    {
        return fieldName switch
        {
            "Id" => Comparer.DefaultInvariant.Compare(Id, other.Id),
            "Name" or "Имя" => Comparer.DefaultInvariant.Compare(Name, other.Name),
            "Position" or "Позиция" => Comparer.DefaultInvariant.Compare(Position, other.Position),
            "Jersey number" or "Игровой номер" => Comparer.DefaultInvariant.Compare(JerseyNumber, other.JerseyNumber),
            "Team" or "Команда" => Comparer.DefaultInvariant.Compare(TeamName, other.TeamName),
            _ => throw new ArgumentOutOfRangeException(nameof(fieldName), fieldName, null)
        };
    }
    
    public string ToJson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(this, options);
    }

    public void OnPlayerUpdated(PlayerUpdatedEventArgs e)
    {
        var temp = Updated;
        temp?.Invoke(this, e);
    }

    public void AttachObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        Updated += observer;
    }

    public void DetachObserver(EventHandler<PlayerUpdatedEventArgs> observer)
    {
        Updated -= observer;
    }

    public int GetBadCardsCount()
        => Stats.Count(stat => stat.ConvertStringTypeToEnum(stat.Type) is StatType.RedCards or StatType.YellowCards);
}