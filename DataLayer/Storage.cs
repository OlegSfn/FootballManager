using BusinessLogic;

namespace DataLayer;

public static class Storage
{
    public static List<GameData> GameDatas;
    public static int CurrentGameIndex = 0;

    public static GameData CurrentGame => GameDatas[CurrentGameIndex];
}