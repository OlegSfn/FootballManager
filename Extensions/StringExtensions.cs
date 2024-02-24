namespace Extensions;

/// <summary>
/// Provides extension methods for string manipulation.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Aligns the string to the left within a specified maximum length.
    /// </summary>
    /// <param name="str">The string to align.</param>
    /// <param name="maxLen">The maximum length of the resulting string.</param>
    /// <returns>The aligned string.</returns>
    public static string AlignLeft(this string str, int maxLen)
    {
        if (maxLen <= 3)
            return "...";
        
        return str.Length <= maxLen ? str.PadRight(maxLen) : str.Remove(maxLen-3).PadRight(maxLen,'.');
    }
    
    /// <summary>
    /// Aligns the string to the right within a specified maximum length.
    /// </summary>
    /// <param name="str">The string to align.</param>
    /// <param name="maxLen">The maximum length of the resulting string.</param>
    /// <returns>The aligned string.</returns>
    public static string AlignRight(this string str, int maxLen)
    {
        if (maxLen <= 3)
            return "...";
        
        return str.Length <= maxLen ? str.PadLeft(maxLen) : str.Remove(0, str.Length-maxLen+3).PadLeft(maxLen,'.');
    }

    /// <summary>
    /// Aligns the string to the center within a specified maximum length.
    /// </summary>
    /// <param name="str">The string to align.</param>
    /// <param name="maxLen">The maximum length of the resulting string.</param>
    /// <returns>The aligned string.</returns>
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