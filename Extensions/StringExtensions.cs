namespace Extensions;

public static class StringExtensions
{
    public static string AlignLeft(this string str, int maxLen)
    {
        if (maxLen <= 3)
            return "...";
        
        return str.Length <= maxLen ? str.PadRight(maxLen) : str.Remove(maxLen-3).PadRight(maxLen,'.');
    }
    
    public static string AlignRight(this string str, int maxLen)
    {
        if (maxLen <= 3)
            return "...";
        
        return str.Length <= maxLen ? str.PadLeft(maxLen) : str.Remove(0, str.Length-maxLen+3).PadLeft(maxLen,'.');
    }

    public static string AlignCenter(this string str, int maxLen)
    {
        if (maxLen <= 3)
            return "...";

        if (str.Length > maxLen)
            return AlignLeft(str, maxLen);

        int padding = (maxLen - str.Length) / 2;
        if ((maxLen - str.Length) % 2 == 0)
            return new string(' ', padding) + str + new string(' ', padding);
        
        return new string(' ', padding) + str + new string(' ', padding+1);
    }
}