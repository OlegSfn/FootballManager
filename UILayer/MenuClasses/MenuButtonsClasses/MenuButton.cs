using UILayer.MenuClasses.MenuButtonGroupsClasses;

namespace UILayer.MenuClasses.MenuButtonsClasses;

/// <summary>
/// Represents a button in a menu.
/// </summary>
public abstract class MenuButton
{
    public string Text { get; protected init; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ButtonsGroup? Group { get; protected set; }
    
    /// <summary>
    /// Registers and proceed the action associated with a specified console key.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public abstract void RegisterAction(ConsoleKey key);
}