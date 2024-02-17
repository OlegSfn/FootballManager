namespace UILayer;

public class Printer
{
    /// <summary>
    /// Clears the console screen.
    /// </summary>
    public static void FullClear()
    {
        Console.Clear();
        Console.Write("\x1b[3J");
    }
    
    /// <summary>
    /// Prints informational message to the console.
    /// </summary>
    /// <param name="msg">The message to print.</param>
    /// <param name="endWithNewLine">Indicates whether to end the message with a new line.</param>
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
    /// Prints a warning message to the console.
    /// </summary>
    /// <param name="msg">The warning message to print.</param>
    /// <param name="endWithNewLine">Indicates whether to end the message with a new line.</param>
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
    /// Prints an error message to the console.
    /// </summary>
    /// <param name="msg">The error message to print.</param>
    /// <param name="endWithNewLine">Indicates whether to end the message with a new line.</param>
    public static void PrintError(string msg, bool endWithNewLine = true)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (endWithNewLine)
            Console.WriteLine(msg);
        else
            Console.Write(msg);

        Console.ResetColor();
    }
    
    /// <summary>
    /// Converts a boolean value to "Да" or "Нет".
    /// </summary>
    /// <param name="b">The boolean value to convert.</param>
    /// <returns>"Да" if true, "Нет" if false.</returns>
    public static string BoolToYesOrNo(bool b) => b ? "Да" : "Нет";

    
    /// <summary>
    /// Prints a line in the table with proper formatting.
    /// </summary>
    /// <param name="line">An array of strings representing a row in the table.</param>
    /// <param name="sep">The character used as a separator in the table.</param>
    /// <param name="startIndex">The starting index of the displayed columns.</param>
    /// <param name="count">The number of columns to display.</param>
    private static void PrintLine(string?[] line, char sep, int startIndex, int count)
    {
        int endIndex = Math.Min(startIndex + count, line.Length);
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i != endIndex-1)
                Console.Write($"{line[i]}{sep}{sep}{sep}{sep}");
            else
                Console.Write($"{line[i]}");
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Creates a readable array of strings with proper formatting for displaying in the table.
    /// </summary>
    /// <param name="columns">An array of strings representing columns in the table.</param>
    /// <param name="maxLen">The maximum length of each column.</param>
    /// <param name="emptyColIndexes">An array of indexes indicating empty columns.</param>
    /// <returns>An array of readable strings for displaying in the table.</returns>
    private static string?[] MakeReadableArray(string?[] columns, int maxLen, int[] emptyColIndexes)
    {
        List<string?> readableArr = new List<string?>();
        if (maxLen < 3)
        {
            for (int i = 0; i < readableArr.Count; i++)
                readableArr[i] = ".";
            return readableArr.ToArray();
        }

        for (int i = 0; i < columns.Length; i++)
        {
            if (emptyColIndexes.Contains(i))
                continue;
            
            if (columns[i] == null)
            {
                readableArr.Add(new string(' ', maxLen));
                continue;
            }
            
            int colLen = columns[i].Length;
            if (colLen > maxLen)
                readableArr.Add(columns[i].Remove(maxLen-3, colLen-maxLen+3) + "...");
            else if (colLen < maxLen)
                readableArr.Add(columns[i].PadRight(maxLen));
            else
                readableArr.Add(columns[i]);
        }

        return readableArr.ToArray();
    }
}