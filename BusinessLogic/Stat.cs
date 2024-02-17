using System.Text.Json.Serialization;

namespace BusinessLogic;

public class Stat
{
    [JsonPropertyName("statId")]
    public string Id { get; init; }
    
    [JsonPropertyName("statType")]
    public string Type { get; init; }

    public Stat(string type)
    {
        Id = GenerateId();
        Type = type;
    }
    
    
    public StatType ConvertStringTypeToEnum(string type) => type switch
    {
        "Yellow Cards" => StatType.YellowCards,
        "Red Cards" => StatType.RedCards,
        "Assists" => StatType.Assists,
        "Goals" => StatType.Goals,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public override string ToString()
    {
        return $"{Id} | {Type}";
    }

    private string GenerateId()
    {
        return "123";
    }
}