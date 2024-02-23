using UILayer.MenuClasses.MenuButtonGroupsClasses;

namespace UILayer.MenuClasses.MenuButtonsClasses;

public abstract class MenuButton
{
    public string Text { get; protected init; }
    public bool IsActive { get; set; } = true;
    public ButtonsGroup Group { get; protected set; }
    
    public abstract void RegisterAction(ConsoleKey key);
}