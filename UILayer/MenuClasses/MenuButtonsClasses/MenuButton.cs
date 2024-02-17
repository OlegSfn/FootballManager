using UILayer.MenuClasses.MenuButtonGroupsClasses;

namespace UILayer.MenuClasses;

public abstract class MenuButton
{
    public string Text { get; protected set; }
    public ButtonsGroup Group { get; protected set; }
    
    public abstract void RegisterAction(ConsoleKey key);
}