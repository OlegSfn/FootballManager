namespace DataLayer;

public class SettingsData
{
    public bool IsFirstUsing { get; set; } = true;
    public string[] SortSaveMode = { "Сохранять в новый файл", "Сохранять в текущий файл", "Спрашивать каждый раз" };
}