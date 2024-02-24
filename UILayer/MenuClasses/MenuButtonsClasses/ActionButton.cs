namespace UILayer.MenuClasses.MenuButtonsClasses;

/// <summary>
/// Represents a button in a menu that performs a specified action when pressed.
/// </summary>
public class ActionButton : MenuButton
{
    private readonly Action _action;

    public ActionButton(string text, Action action)
    {
        Text = text;
        _action = action ?? (() => { });
    }

    public ActionButton(string text) : this(text, () => { }) { }
    public ActionButton() { }
    
    /// <summary>
    /// Invoke Action that is associated with this button.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
            _action?.Invoke();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Text}";
    }
}

/// <summary>
/// Represents a button in a menu that performs a specified action with a context when activated.
/// </summary>
/// <typeparam name="T">The type of the context for the action.</typeparam>
public class ActionButton<T> : MenuButton
{
    private readonly Action<T> _action;
    private readonly T _context;

    public ActionButton(string text, Action<T> action, T context)
    {
        Text = text;
        _action = action ?? (_ => { });
        _context = context;
    }

    public ActionButton(string text, T context) : this(text, _ => { }, context) { }

    public ActionButton() { }

    /// <summary>
    /// Invoke Action that is associated with this button.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Enter)
            _action?.Invoke(_context);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Text}";
    }
}