using Extensions;
using UILayer.MenuClasses.MenuButtonGroupsClasses;
using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses;

/// <summary>
/// Represents a menu in a user interface.
/// </summary>
public class Menu
{
    private readonly ButtonsGroup[] _groups;
    private int _groupIndex;
    private ButtonsGroup SelectedGroup => _groups[_groupIndex];
    private readonly string _header;
    private readonly AlignMode _headerAlign;
    private readonly AlignMode _buttonsAlign;
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
    
    public Menu() {}

    /// <summary>
    /// Handles the interaction with the menu.
    /// </summary>
    /// <returns><c>true</c> if the menu interaction needed to be repeated; otherwise, <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown when all groups are inactive.</exception>
    public bool HandleUsing()
    {
        if (!SelectedGroup.IsActive || !SelectedGroup.MenuButtons[SelectedGroup.CursorPosition].IsActive)
        {
            var findNewPos = false;
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
            var pressedKey = Console.ReadKey().Key;
            switch (pressedKey)
            {
                case ConsoleKey.DownArrow:
                    MoveCursorDown();
                    break;
                case ConsoleKey.UpArrow:
                    MoveCursorUp();
                    break;
                case ConsoleKey.Tab:
                    return false;
                case ConsoleKey.Enter:
                    PushSelectedButton(pressedKey);
                    return true;
                default:
                    PushSelectedButton(pressedKey);
                    break;
            }
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
                    AlignPrint(groupButtons[j].ToString()!, _buttonsAlign);
                    Console.ResetColor();
                }
                else
                    AlignPrint(groupButtons[j].ToString()!, _buttonsAlign);
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

    private static void AlignPrint(string text, AlignMode alignMode)
    {
        switch (alignMode)
        {
            case AlignMode.Left:
                Console.WriteLine(text.AlignLeft(Console.WindowWidth));
                break;
            case AlignMode.Right:
                Console.WriteLine(text.AlignRight(Console.WindowWidth));
                break;
            case AlignMode.Center:
                Console.WriteLine(text.AlignCenter(Console.WindowWidth));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(alignMode), alignMode, null);
        }
    }
}