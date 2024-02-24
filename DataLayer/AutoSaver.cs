using BusinessLogic.EventArgs;

namespace DataLayer;

/// <summary>
/// Automatically saves game data when a player is updated.
/// </summary>
public class AutoSaver
{
    private DateTime _lastTimeUpdated = DateTime.MinValue;

    /// <summary>
    /// Saves current game data if the changes were made less or equal than 15 seconds.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments containing the update information.</param>
    public void PlayerChangedHandler(object? sender, PlayerUpdatedEventArgs e)
    {
        if ((e.UpdateTime - _lastTimeUpdated).Seconds <= 15)
            Storage.CurrentGame.SaveToFile(Path.GetFileNameWithoutExtension(Storage.CurrentGame.FileName) + "_tmp.json");
        
        _lastTimeUpdated = e.UpdateTime;
    }
}