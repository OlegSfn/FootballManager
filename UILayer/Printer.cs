namespace UILayer;

/// <summary>
/// Provides methods for printing messages to the console with different colors and styles.
/// </summary>
public static class Printer
{
    /// <summary>
    /// Prints an informational message in green color.
    /// </summary>
    /// <param name="msg">The message to print.</param>
    /// <param name="endWithNewLine">Specifies whether the message should end with a new line character. Default is true.</param>
    public static void PrintInfo(string msg, bool endWithNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        if (endWithNewLine)
            Console.WriteLine(msg);
        else
            Console.Write(msg);

        Console.ResetColor();
    }
    
    /// <summary>
    /// Prints a warning message in yellow color.
    /// </summary>
    /// <param name="msg">The message to print.</param>
    /// <param name="endWithNewLine">Specifies whether the message should end with a new line character. Default is true.</param>
    public static void PrintWarning(string msg, bool endWithNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        if (endWithNewLine)
            Console.WriteLine(msg);
        else
            Console.Write(msg);

        Console.ResetColor();
    }
    
    /// <summary>
    /// Prints an error message in red color.
    /// </summary>
    /// <param name="msg">The message to print.</param>
    /// <param name="endWithNewLine">Specifies whether the message should end with a new line character. Default is true.</param>
    public static void PrintError(string msg, bool endWithNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (endWithNewLine)
            Console.WriteLine(msg);
        else
            Console.Write(msg);

        Console.ResetColor();
    }
}