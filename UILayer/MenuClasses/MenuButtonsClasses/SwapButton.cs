namespace UILayer.MenuClasses;

public class SwapButton : MenuButton
{
    public string[] SwapVariants { get; private set; }
    public string CurVariant => SwapVariants[_curVariantIndex];
    private int _curVariantIndex;

    public SwapButton(string text, string[] swapVariants)
    {
        Text = text;
        SwapVariants = swapVariants;
    }
    
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow)
            _curVariantIndex = Math.Clamp(_curVariantIndex - 1, 0, SwapVariants.Length);
        else if (key == ConsoleKey.RightArrow)
            _curVariantIndex = Math.Clamp(_curVariantIndex + 1, 0, SwapVariants.Length-1);
    }

    public override string ToString()
    {
        return $"{Text} ({SwapVariants[_curVariantIndex]})";
    }
}