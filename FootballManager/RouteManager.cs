using System.Text.Json;
using BusinessLogic;
using UILayer.MenuClasses.MenuButtonsClasses;
using DataLayer;
using UILayer;
using UILayer.MenuClasses;

namespace FootballManager;

public class RouteManager
{
    public void EnterMainMenu()
    {
        ActionButton enterNewDataButton = new ActionButton("Ввести данные о новой игре из файла", EnterNewData);
        ActionButton changeCurrentGameButton = new ActionButton($"Изменить текущую игру ({Storage.CurrentGame})", ChangeCurrentGame);
        ActionButton sortPlayersButton = new ActionButton("Отсортировать игроков", SortPlayers);
        ActionButton changePlayerDataButton = new ActionButton("Изменить данные об игроке", ChangePlayerData);
        ActionButton settingsButton = new ActionButton("Настройки", Settings);
        ActionButton helpButton = new ActionButton("Помощь", Help);
        ActionButton exitButton = new ActionButton("Выход", () => Environment.Exit(0));

        MenuButton[] menuButtons = {enterNewDataButton, changeCurrentGameButton, sortPlayersButton,changePlayerDataButton,  settingsButton, helpButton, exitButton};
        
        Menu menu = new Menu("Меню:", menuButtons);
        menu.HandleUsing();
    }

    private void EnterNewData()
    {
        string? filePath = InputHandler.GetFilePathToJson("Введите путь до файла с данными: ");
        if (filePath == null)
            return;
        List<Player>? players = JsonSerializer.Deserialize<List<Player>>(filePath);
        
    }

    private void ChangeCurrentGame()
    {
        
    }

    private void SortPlayers()
    {
        
    }

    private void ChangePlayerData()
    {
        
    }

    private void Settings()
    {
        
    }

    private void Help()
    {
        
    }
}