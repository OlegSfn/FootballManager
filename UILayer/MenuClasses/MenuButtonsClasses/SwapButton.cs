using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses;

public class SwapButton<T> : MenuButton
{
    public T[] SwapVariants { get; private set; }
    public T CurVariant => SwapVariants[_curVariantIndex];
    private int _curVariantIndex;
    
    private bool _isCycled;

    private Action<T> _swapAction;
    private Action<T> _confirmAction;

    public SwapButton(string text, T[] swapVariants, Action<T>? swapAction = null, Action<T>? confirmAction = null, bool isCycled = false)
    {
        Text = text;
        SwapVariants = swapVariants;
        _swapAction = swapAction ?? (_ => {});
        _confirmAction = confirmAction ?? (_ => {});
        _isCycled = isCycled;
    }

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

    public override string ToString()
    {
        return $"<-{Text} ({SwapVariants[_curVariantIndex]})->";
    }
}