using System.Text;
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