using BusinessLogic;

namespace DataLayer;

/// <summary>
/// Provides storage functionality for game data.
/// </summary>
public static class Storage
{
    public static readonly List<Game> GameDatas = new();
    public static Game CurrentGame { get; set; } = null!;
}