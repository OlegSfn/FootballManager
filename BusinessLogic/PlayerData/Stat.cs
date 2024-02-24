using System.Text;
using System.Text.Json.Serialization;

namespace BusinessLogic.PlayerData;

/// <summary>
/// Represents a statistical data associated with a player.
/// </summary>
public class Stat
{
    [JsonPropertyName("statId")]
    public string Id { get; init; }
    
    [JsonPropertyName("statType")]
    public string Type { get; init; }

    [JsonConstructor]
    public Stat(string id, string type)
    {
        Id = id;
        Type = type;
    }
    
    public Stat(string type) : this(GenerateId(), type) { }
    public Stat() : this("Goals")  { }
    
    /// <summary>
    /// Gets the enumeration type corresponding to the statistical data type.
    /// </summary>
    /// <returns>The enumeration type of the statistical data.</returns>
    public StatType GetEnumType() => Type switch
    {
        "Yellow Cards" => StatType.YellowCards,
        "Red Cards" => StatType.RedCards,
        "Assists" => StatType.Assists,
        "Goals" => StatType.Goals,
        _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
    };

    private static string GenerateId()
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz1234567890";
        var res = new StringBuilder();
        var rnd = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < 8; i++)
            res.Append(alphabet[rnd.Next(alphabet.Length)]);

        res.Append('-');
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
                res.Append(alphabet[rnd.Next(alphabet.Length)]);
            res.Append('-');
        }
        
        for (int i = 0; i < 12; i++)
            res.Append(alphabet[rnd.Next(alphabet.Length)]);
        
        return res.ToString();
    }
}