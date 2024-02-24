namespace UILayer.MenuClasses.MenuButtonsClasses;
using MenuButtonGroupsClasses;

/// <summary>
/// Represents a button in a menu that behaves like a toggle button.
/// </summary>
public class ToggleButton : MenuButton
{
    private ToggleButtonsGroup _toggleButtonGroup;
    
    public bool IsSelected { get; private set; }

    public ToggleButton(string text, ToggleButtonsGroup toggleButtonGroup)
    {
        Text = text;
        _toggleButtonGroup = toggleButtonGroup;
    }
    
    /// <summary>
    /// Selects or deselects this button in her ToggleButtonsGroup.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Spacebar)
        {
            IsSelected = !IsSelected;
            _toggleButtonGroup.UpdateToggledButtons(this);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{(IsSelected ? '\u25a0' : '\u25a1')} {Text}";
    }
}