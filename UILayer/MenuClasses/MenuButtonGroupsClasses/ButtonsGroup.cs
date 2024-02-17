namespace UILayer.MenuClasses.MenuButtonGroupsClasses;

public class ButtonsGroup
{
    public MenuButton[] MenuButtons { get; set; }
    public int CursorPosition { get; set; }
    public bool IsActive { get; set; } = true;

    public bool CanMoveCursorDown()
        => CursorPosition != MenuButtons.Length - 1;

    public bool CanMoveCursorUp()
        => CursorPosition != 0;
}