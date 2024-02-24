namespace UILayer;

/// <summary>
/// Provides methods for handling user input.
/// </summary>
public static class InputHandler
{
    /// <summary>
    /// Attempts to parse an integer value from user input.
    /// </summary>
    /// <param name="msg">The message to display as a prompt for user input.</param>
    /// <param name="newMsg">The message to display if the input is invalid and needs to be re-prompted.</param>
    /// <param name="result">When this method returns, contains the integer value entered by the user, if the parsing succeeded, or -1 if the parsing failed.</param>
    /// <returns>True if the input was successfully parsed as an integer; otherwise, false.</returns>
    public static bool GetIntValue(string msg, string newMsg, out int result)
    {
        result = -1;
        while (true)
        {
            Console.Write(msg);
            var userInp = Console.ReadLine();
            if (userInp == null)
                return false;
            
            if (int.TryParse(userInp, out result))
                return true;

            Printer.PrintError("Вы ввели не целое число.");
            msg = newMsg;
        }
    }
    
    /// <summary>
    /// Waits for the user to press any key.
    /// </summary>
    /// <param name="msg">The message to display before waiting.</param>
    public static void WaitForUserInput(string msg)
    {
        Printer.PrintWarning(msg, false);
        Console.ReadKey(true);
        Console.WriteLine();
    }
    
    /// <summary>
    /// Prompts the user to enter a valid file path to a JSON file, ensuring it has the correct extension.
    /// </summary>
    /// <param name="msg">The message to display as a prompt for the user to enter the file path.</param>
    /// <returns>
    /// A valid file path to a JSON file entered by the user, or null if the user provides no input.
    /// </returns>
    public static string? GetFilePathToJson(string msg)
    {
        while (true)
        {
            Console.Write(msg);
            var filePath = Console.ReadLine();
            if (filePath == null)
                return null;
            
            filePath = filePath.EndsWith(".json") ? filePath : filePath + ".json";
            if (filePath.All(x => !Path.GetInvalidPathChars().Contains(x)))
            {
                if (File.Exists(filePath))
                    return filePath;
                
                Printer.PrintError("Файл по указанному пути не существует.");
            }
            else
                Printer.PrintError("Название файла содержит недопустимые символы.");
        }
    }
}