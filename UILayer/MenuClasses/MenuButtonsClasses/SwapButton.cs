namespace UILayer.MenuClasses.MenuButtonsClasses;

/// <summary>
/// Represents a button in a menu that allows swapping between multiple variants.
/// </summary>
/// <typeparam name="T">The type of the variants.</typeparam>
public class SwapButton<T> : MenuButton
{
    public T[] SwapVariants { get; init; }
    public T CurVariant => SwapVariants[_curVariantIndex];
    private int _curVariantIndex;
    
    private readonly bool _isCycled;

    private readonly Action<T> _swapAction;
    private readonly Action<T> _confirmAction;

    public SwapButton(string text, T[] swapVariants, Action<T>? swapAction = null, Action<T>? confirmAction = null, bool isCycled = false, int startIndex = 0)
    {
        Text = text;
        SwapVariants = swapVariants;
        _swapAction = swapAction ?? (_ => {});
        _confirmAction = confirmAction ?? (_ => {});
        _isCycled = isCycled;
        _curVariantIndex = startIndex;
    }

    /// <summary>
    /// Invoke ConfirmAction that is associated with this button, when pressed, Invoke SwapAction that is associated with this button, when swapped.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow)
        {
            if (_isCycled)
                _curVariantIndex = (_curVariantIndex - 1 + SwapVariants.Length) % SwapVariants.Length;
            else
                _curVariantIndex = Math.Clamp(_curVariantIndex - 1, 0, SwapVariants.Length);
            _swapAction?.Invoke(CurVariant);
        }
        else if (key == ConsoleKey.RightArrow)
        {
            if (_isCycled)
                _curVariantIndex = (_curVariantIndex + 1) % SwapVariants.Length;
            else
                _curVariantIndex = Math.Clamp(_curVariantIndex + 1, 0, SwapVariants.Length-1);
            _swapAction?.Invoke(CurVariant);
        }
        else if (key == ConsoleKey.Enter)
            _confirmAction.Invoke(CurVariant);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"<-{Text} ({SwapVariants[_curVariantIndex]})->";
    }
}