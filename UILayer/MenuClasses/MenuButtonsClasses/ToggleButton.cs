namespace UILayer.MenuClasses.MenuButtonsClasses;
using MenuButtonGroupsClasses;

public class ToggleButton : MenuButton
{
    private ToggleButtonsGroup _toggleButtonGroup;
    
    public bool IsSelected { get; private set; }

    public ToggleButton(string text, ToggleButtonsGroup toggleButtonGroup)
    {
        Text = text;
        _toggleButtonGroup = toggleButtonGroup;
    }
    
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Spacebar)
        {
            IsSelected = !IsSelected;
            _toggleButtonGroup.UpdateToggledButtons(this);
        }
    }

    public override string ToString()
    {
        return $"{(IsSelected ? '\u25a0' : '\u25a1')} {Text}";
    }
}