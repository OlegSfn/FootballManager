namespace UILayer.MenuClasses.MenuButtonsClasses;

public class ActionButton : MenuButton
{
    private readonly Action _action;

    public ActionButton(string text, Action action)
    {
        Text = text;
        _action = action;
    }

    public ActionButton(string text) : this(text, () => { }) { }
    

    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
            _action?.Invoke();
    }

    public override string ToString()
    {
        return $"{Text}";
    }
}

public class ActionButton<T> : MenuButton
{
    private readonly Action<T> _action;
    private readonly T _context;

    public ActionButton(string text, Action<T> action, T context)
    {
        Text = text;
        _action = action;
        _context = context;
    }

    public ActionButton(string text, T context) : this(text, _ => { }, context) { }
    
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
            _action?.Invoke(_context);
    }

    public override string ToString()
    {
        return $"{Text}";
    }
}