using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses.MenuButtonGroupsClasses;

/// <summary>
/// Represents a group of menu buttons.
/// </summary>
public class ButtonsGroup
{
    public MenuButton[] MenuButtons { get; set; }
    public int CursorPosition { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Determines whether the cursor can move down within the group.
    /// </summary>
    /// <returns><c>true</c> if the cursor can move down; otherwise, <c>false</c>.</returns>
    public bool CanMoveCursorDown()
        => CursorPosition != FindNewCursorPosition(1);

    /// <summary>
    /// Determines whether the cursor can move up within the group.
    /// </summary>
    /// <returns><c>true</c> if the cursor can move up; otherwise, <c>false</c>.</returns>
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