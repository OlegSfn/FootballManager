using Extensions;
using UILayer.MenuClasses.MenuButtonGroupsClasses;
using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses;

/// <summary>
/// Represents a menu in a user interface.
/// </summary>
public class Menu
{
    private ButtonsGroup[] _groups;
    private int _groupIndex;
    private ButtonsGroup SelectedGroup => _groups[_groupIndex];
    private string _header;
    private AlignMode _headerAlign;
    private AlignMode _buttonsAlign;
    public bool ShowSelected { get; set; } = true;

    //TODO: Force user to choose at least one of the radiobutton.
    public Menu(MenuButton[] buttons, string header = "", AlignMode headerAlign = AlignMode.Center, AlignMode buttonsAlign = AlignMode.Center)
    {
        _header = header;
        _groups = new ButtonsGroup[] {new()};
        _groups[0].MenuButtons = buttons;
        _groupIndex = 0;
        _headerAlign = headerAlign;
        _buttonsAlign = buttonsAlign;
    }

    public Menu(ButtonsGroup[] groups, string header = "", AlignMode headerAlign = AlignMode.Center, AlignMode buttonsAlign = AlignMode.Center)
    {
        _header = header;
        _groups = groups;
        _groupIndex = 0;
        _headerAlign = headerAlign;
        _buttonsAlign = buttonsAlign;
    }

    /// <summary>
    /// Handles the interaction with the menu.
    /// </summary>
    /// <returns><c>true</c> if the menu interaction needed to be repeated; otherwise, <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown when all groups are inactive.</exception>
    public bool HandleUsing()
    {
        if (!SelectedGroup.IsActive || !SelectedGroup.MenuButtons[SelectedGroup.CursorPosition].IsActive)
        {
            bool findNewPos = false;
            for (int i = 0; i < _groups.Length; i++)
            {
                if (!_groups[i].IsActive) continue;
                
                for (int j = 0; j < _groups[i].MenuButtons.Length; j++)
                {
                    if (_groups[i].MenuButtons[j].IsActive)
                    {
                        _groupIndex = i;
                        _groups[i].CursorPosition = j;
                        findNewPos = true;
                        break;
                    }
                }
                
                if (findNewPos)
                    break;
            }
            
            if (SelectedGroup.IsActive == false)
                throw new Exception("All groups are inactive");
        }
        
        while (true)
        {
            Show();
            ConsoleKey pressedKey = Console.ReadKey().Key;
            if (pressedKey == ConsoleKey.DownArrow)
                MoveCursorDown();
            else if (pressedKey == ConsoleKey.UpArrow)
                MoveCursorUp();
            else if (pressedKey == ConsoleKey.Tab)
                return false;
            else if (pressedKey == ConsoleKey.Enter)
            {
                PushSelectedButton(pressedKey);
                return true;
            }
            else
                PushSelectedButton(pressedKey);
        }
    }

    private void Show()
    {
        Console.Clear();
        if (_header != string.Empty)
            AlignPrint(_header, _headerAlign);
        
        foreach (var group in _groups)
        {
            var groupButtons = group.MenuButtons;
            for (int j = 0; j < groupButtons.Length; j++)
            {
                if (!group.IsVisible || !groupButtons[j].IsActive) continue;
                
                if (group == SelectedGroup && j == group.CursorPosition && ShowSelected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    AlignPrint(groupButtons[j].ToString()!, _headerAlign);
                    Console.ResetColor();
                }
                else
                    AlignPrint(groupButtons[j].ToString()!, _headerAlign);
            }
        }
    }

    private void MoveCursorDown()
    {
        if (SelectedGroup.CanMoveCursorDown())
            SelectedGroup.CursorPosition++;
        else
            _groupIndex = FindNewGroupIndex(1);
    }

    private void MoveCursorUp()
    {
        if (SelectedGroup.CanMoveCursorUp())
            SelectedGroup.CursorPosition--;
        else
            _groupIndex = FindNewGroupIndex(-1);
    }

    private void PushSelectedButton(ConsoleKey pressedKey)
    {
        SelectedGroup.MenuButtons[SelectedGroup.CursorPosition].RegisterAction(pressedKey);
    }

    private int FindNewGroupIndex(int delta)
    {
        var newGroupIndex = _groupIndex+delta;
        while (newGroupIndex < _groups.Length && newGroupIndex >= 0)
        {
            if (_groups[newGroupIndex].IsActive)
                return newGroupIndex;
            
            newGroupIndex += delta;
        }

        return _groupIndex;
    }

    private void AlignPrint(string text, AlignMode alignMode)
    {
        if (alignMode == AlignMode.Left)
            Console.WriteLine(text.AlignLeft(Console.WindowWidth));
        if (alignMode == AlignMode.Right)
            Console.WriteLine(text.AlignRight(Console.WindowWidth));
        if (alignMode == AlignMode.Center) 
            Console.WriteLine(text.AlignCenter(Console.WindowWidth));
    }
}