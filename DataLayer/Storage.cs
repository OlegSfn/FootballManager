using BusinessLogic;

namespace DataLayer;

public static class Storage
{
    public static List<GameData> GameDatas = new();
    public static GameData CurrentGame { get; set; }
}