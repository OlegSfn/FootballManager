using System.Text.Json;
using UILayer;

namespace DataLayer;

public static class Settings
{
    public static SettingsData? SettingsOptions { get; set; }
    private static readonly string s_settingsFileName = "settings.txt";
    
    private static void SaveSettings(bool silentSave)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(SettingsOptions, options);
            File.WriteAllText(s_settingsFileName, jsonString);
            if (!silentSave)
                Printer.PrintInfo("Настройки сохранены.");
        }
        catch (IOException)
        {
            Printer.PrintError("Произошла ошибка при сохранении файла настроек.");
        }
    }

    public static void LoadSettings()
    {

        if (!File.Exists(s_settingsFileName))
        {
            LoadDefaultSettings();
            return;
        }
        
        try
        {
            string jsonData = File.ReadAllText(s_settingsFileName);
            SettingsOptions = JsonSerializer.Deserialize<SettingsData>(jsonData);
        }
        catch (IOException)
        {
            Console.WriteLine("Произошла ошибка при чтении настроек, установлены стандартные настройки.");
            LoadDefaultSettings();
        }
    }

    private static void LoadDefaultSettings()
    {
        SettingsOptions = new SettingsData();
        SaveSettings(true);
    }

    public static void SetCorrectEncoding()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
    }
}