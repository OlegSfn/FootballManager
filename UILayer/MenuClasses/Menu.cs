namespace UILayer.MenuClasses;

public class Menu
{
    private MenuButton[] _buttons;
    private int _cursorPosition;
    private string _header;

    public Menu(string header, MenuButton[] buttons)
    {
        _header = header;
        _buttons = buttons;
    }

    public Menu(MenuButton[] buttons) : this(string.Empty, buttons){ }

    public void HandleUsing()
    {
        while (true)
        {
            Show();
            ConsoleKey pressedKey = Console.ReadKey().Key;
            if (pressedKey == ConsoleKey.DownArrow)
                _cursorPosition = Math.Clamp(_cursorPosition + 1, 0, _buttons.Length-1);
            else if (pressedKey == ConsoleKey.UpArrow)
                _cursorPosition = Math.Clamp(_cursorPosition - 1, 0, _buttons.Length);
            else if (pressedKey == ConsoleKey.Enter)
            {
                _buttons[_cursorPosition].RegisterAction(pressedKey);
                return;
            }
            else
                _buttons[_cursorPosition].RegisterAction(pressedKey);
        }
    }

    private void Show()
    {
        Console.Clear();
        if (_header != string.Empty)
            Console.WriteLine(_header);
        
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == _cursorPosition)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(_buttons[i]);
                Console.ResetColor();
            }
            else
                Console.WriteLine(_buttons[i]);
        }
    }
    
}