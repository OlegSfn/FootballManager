using BusinessLogic.EventArgs;

namespace DataLayer;

public class AutoSaver
{
    private DateTime _lastTimeUpdated = DateTime.MinValue;

    public void PlayerChangedHandler(object? sender, PlayerUpdatedEventArgs e)
    {
        if ((e.UpdateTime - _lastTimeUpdated).Seconds <= 15)
            Storage.CurrentGame.SaveToFile(Path.GetFileNameWithoutExtension(Storage.CurrentGame.FileName) + "_tmp.json");
        
        _lastTimeUpdated = e.UpdateTime;
    }
}