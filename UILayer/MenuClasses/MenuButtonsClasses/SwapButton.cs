namespace UILayer.MenuClasses;

public class SwapButton<T> : MenuButton
{
    public T[] SwapVariants { get; private set; }
    public T CurVariant => SwapVariants[_curVariantIndex];
    private int _curVariantIndex;
    
    private bool _isCycled;

    private Action<T> _action;

    public SwapButton(string text, T[] swapVariants, Action<T> action, bool isCycled = false)
    {
        Text = text;
        SwapVariants = swapVariants;
        _action = action;
        _isCycled = isCycled;
    }

    public SwapButton(string text, T[] swapVariants, bool isCycled = false) : this(text, swapVariants, _ => { }, isCycled:isCycled) { }

    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow)
        {
            if (_isCycled)
                _curVariantIndex = (_curVariantIndex - 1 + SwapVariants.Length) % SwapVariants.Length;
            else
                _curVariantIndex = Math.Clamp(_curVariantIndex - 1, 0, SwapVariants.Length);
            _action?.Invoke(CurVariant);
        }
        else if (key == ConsoleKey.RightArrow)
        {
            if (_isCycled)
                _curVariantIndex = (_curVariantIndex + 1) % SwapVariants.Length;
            else
                _curVariantIndex = Math.Clamp(_curVariantIndex + 1, 0, SwapVariants.Length-1);
            _action?.Invoke(CurVariant);
        }
    }

    public override string ToString()
    {
        return $"<-{Text} ({SwapVariants[_curVariantIndex]})->";
    }
}