namespace UILayer.MenuClasses;

public class SwapButton : MenuButton
{
    public List<string> SwapVariants { get; private set; }
    private int _curVariantIndex;

    public SwapButton(string text, List<string> swapVariants)
    {
        Text = text;
        SwapVariants = swapVariants;
    }
    
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow)
            _curVariantIndex = Math.Clamp(_curVariantIndex - 1, 0, SwapVariants.Count);
        else if (key == ConsoleKey.RightArrow)
            _curVariantIndex = Math.Clamp(_curVariantIndex + 1, 0, SwapVariants.Count-1);
    }

    public override string ToString()
    {
        return $"{Text} ({SwapVariants[_curVariantIndex]})";
    }
}