using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses.MenuButtonGroupsClasses;

public class ButtonsGroup
{
    public MenuButton[] MenuButtons { get; set; }
    public int CursorPosition { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    public bool CanMoveCursorDown()
        => CursorPosition != FindNewCursorPosition(1);

    public bool CanMoveCursorUp()
        => CursorPosition != FindNewCursorPosition(-1);
    
    private int FindNewCursorPosition(int delta)
    {
        var newCursorPosition = CursorPosition+delta;
        while (newCursorPosition < MenuButtons.Length && newCursorPosition >= 0)
        {
            if (MenuButtons[newCursorPosition].IsActive)
                return newCursorPosition;
            
            newCursorPosition += delta;
        }

        return CursorPosition;
    }
}