using Extensions;
using UILayer.MenuClasses.MenuButtonGroupsClasses;
using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses;

public class Menu
{
    private ButtonsGroup[] _groups;
    private int _groupIndex;
    private ButtonsGroup SelectedGroup => _groups[_groupIndex];
    private string _header;
    private AlignMode _headerAlign = AlignMode.Center;
    private AlignMode _buttonsAlign = AlignMode.Center;
    public bool ShowSelected { get; set; } = true;

    //TODO: Force user to choose at least one of the radiobutton.
    public Menu(string header, MenuButton[] buttons)
    {
        _header = header;
        _groups = new ButtonsGroup[] {new()};
        _groups[0].MenuButtons = buttons;
        _groupIndex = 0;
    }

    public Menu(string header, ButtonsGroup[] groups)
    {
        _header = header;
        _groups = groups;
        _groupIndex = 0;
    }

    public Menu(MenuButton[] buttons) : this(string.Empty, buttons){ }
    public Menu(ButtonsGroup[] groups) : this(string.Empty, groups){ }

    public Menu(MenuButton button) : this(string.Empty, new [] { button }) { }

    public bool HandleUsing()
    {
        if (SelectedGroup.IsActive == false)
        {
            for (int i = 0; i < _groups.Length; i++)
            {
                if (_groups[i].IsActive)
                {
                    _groupIndex = i;
                    //TODO: find visible menu point and place cursor on it;
                    break;
                }
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