namespace UILayer.MenuClasses.MenuButtonsClasses;

public class ActionButton : MenuButton
{
    private Action _action;

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