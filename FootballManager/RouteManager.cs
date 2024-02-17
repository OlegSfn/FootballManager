using System.Text.Json;
using BusinessLogic;
using BusinessLogic.EventArgs;
using UILayer.MenuClasses.MenuButtonsClasses;
using DataLayer;
using UILayer;
using UILayer.MenuClasses;
using UILayer.MenuClasses.MenuButtonGroupsClasses;

namespace FootballManager;

public static class RouteManager
{
    public static void EnterMainMenu()
    {
        var enterNewDataButton = new ActionButton("Ввести данные о новой игре из файла", EnterNewData);
        var changeCurrentGameButton =
            new ActionButton($"Изменить текущую игру ({Storage.CurrentGame})", ChangeCurrentGame);
        var sortPlayersButton = new ActionButton("Отсортировать игроков", SortPlayers);
        var changePlayerDataButton = new ActionButton("Изменить данные об игроке", ChangePlayerData);
        var settingsButton = new ActionButton("Настройки", Settings);
        var helpButton = new ActionButton("Помощь", Help);
        var exitButton = new ActionButton("Выход", () => Environment.Exit(0));

        MenuButton[] menuButtons =
        {
            enterNewDataButton, changeCurrentGameButton, sortPlayersButton, changePlayerDataButton, settingsButton,
            helpButton, exitButton
        };

        var menu = new Menu("Меню:", menuButtons);
        menu.HandleUsing();
    }

    private static void EnterNewData()
    {
        string? filePath = InputHandler.GetFilePathToJson("Введите путь до файла с данными: ");
        if (filePath == null)
            return;

        var players = JsonSerializer.Deserialize<List<Player>>(File.ReadAllText(filePath));
        if (players != null)
        {
            var gameData = new GameData(players, Path.GetFileName(filePath));
            Storage.GameDatas.Add(gameData);
            Storage.CurrentGame = gameData;
            AutoSaver autoSaver = new AutoSaver();
            gameData.AttachObserverToAll(autoSaver.PlayerChangedHandler);
        }
        else
            Console.WriteLine("Не найдены данные об игроках в файле.");
    }

    private static void ChangeCurrentGame()
    {
        throw new NotImplementedException();
    }

    private static void SortPlayers()
    {
        var sortTypeRadioGroup = new RadioButtonsGroup();
        var sortByIdButton = new RadioButton("Id", sortTypeRadioGroup);
        var sortByNameButton = new RadioButton("Имя", sortTypeRadioGroup);
        var sortByPositionButton = new RadioButton("Позиция", sortTypeRadioGroup);
        var sortByJerseyNumButton = new RadioButton("Игровой номер", sortTypeRadioGroup);
        var sortByTeamButton = new RadioButton("Команда", sortTypeRadioGroup);
        MenuButton[] sortPlayersTypeButton =
            { sortByIdButton, sortByNameButton, sortByPositionButton, sortByJerseyNumButton, sortByTeamButton };
        var sortTypeMenu = new Menu("Выберите параметр, по которому нужно отсортировать игроков:",
            sortPlayersTypeButton);
        sortTypeMenu.HandleUsing();

        var sortModeRadioGroup = new RadioButtonsGroup();
        var sortByAscendingButton = new RadioButton("По возрастанию", sortModeRadioGroup);
        var sortByDescendingButton = new RadioButton("По убыванию", sortModeRadioGroup);
        MenuButton[] sortPlayersModeButton = { sortByAscendingButton, sortByDescendingButton };
        var sortModeMenu = new Menu("Выберите параметр, по которому нужно отсортировать игроков:",
            sortPlayersModeButton);
        sortModeMenu.HandleUsing();

        bool isReversed = sortModeRadioGroup.SelectedButton.Text == "По убыванию";
        Player[] sortedPlayers = Storage.CurrentGame.SortPlayers(sortTypeRadioGroup.SelectedButton.Text, isReversed);
        GameData.SaveToFile("test.json", sortedPlayers.ToList());
    }

    private static void ChangePlayerData()
    {
        Player chosenPlayer = null!;
        MenuButton[] playerButtons = Storage.CurrentGame.Players.Select(
            x => 
                new ActionButton<Player>($"{x.Id} | {x.Name} | {x.TeamName} | {x.JerseyNumber}", player => chosenPlayer = player, x))
            .ToArray();
        var playersMenu = new Menu("Выберите игрока, данные о котором вы хотите поменять: ", playerButtons);
        playersMenu.HandleUsing();


        var changeNameButton = new ActionButton("Имя", () => ChangePlayerName(chosenPlayer));
        var changePositionButton = new ActionButton("Позиция", () => ChangePlayerPosition(chosenPlayer));
        var changeJerseyNumButton = new ActionButton("Игровой номер", () => ChangePlayerJerseyNumber(chosenPlayer));
        var changeTeamButton = new ActionButton("Команда", () => ChangePlayerTeam(chosenPlayer));
        var changeStatsButton = new ActionButton("Статистика", () => ChangePlayerStats(chosenPlayer));
        MenuButton[] changePlayerButtons =
            { changeNameButton, changePositionButton, changeJerseyNumButton, changeTeamButton, changeStatsButton };
        var changeFieldMenu = new Menu("Выберите параметр, по которому нужно отсортировать игроков:",
            changePlayerButtons);
        changeFieldMenu.HandleUsing();
    }

    private static void ChangePlayerName(Player playerToChange)
    {
        Console.Write("Введите новое имя: ");
        string? newName = Console.ReadLine();
        if (newName != null)
            playerToChange.Name = newName;
    }

    private static void ChangePlayerPosition(Player playerToChange)
    {
        SwapButton positionsButton = new SwapButton("Новая позиция", Storage.CurrentGame.Positions);
        Menu changePositionMenu = new Menu(new MenuButton[] {positionsButton});
        changePositionMenu.HandleUsing();

        playerToChange.Position = positionsButton.CurVariant;
    }
    
    private static void ChangePlayerJerseyNumber(Player playerToChange)
    {
        Console.Write("Введите новый номер: ");
        int newJerseyNumber = InputHandler.GetIntValue("Введите целое число для номера: ", "Введите новое целое число для номера: ");
        playerToChange.JerseyNumber = newJerseyNumber;
    }

    //TODO: Check null
    private static void ChangePlayerTeam(Player playerToChange)
    {
        string[] teamsNames = Storage.CurrentGame.Teams.Select(x => x.Name).Append("Создать новую команду").ToArray();
        var teamsButton = new SwapButton("Новая команда для игрока", teamsNames);
        var changeTeamMenu = new Menu(new MenuButton[] { teamsButton });
        changeTeamMenu.HandleUsing();

        string newTeamName = teamsButton.CurVariant;
        if (newTeamName == "Создать новую команду")
        {
            Console.Write("Введите название для новой команды: ");
            newTeamName = Console.ReadLine();
            if (Storage.CurrentGame.Teams.All(x => x.Name != newTeamName))
                Storage.CurrentGame.CreateNewTeam(newTeamName);
        }
        
        playerToChange.DetachObserver(playerToChange.Team.PlayerChangedHandler);
        var newTeam = Storage.CurrentGame.Teams.First(x => x.Name == newTeamName);
        playerToChange.Team = newTeam;
        playerToChange.TeamName = newTeamName;
        playerToChange.AttachObserver(newTeam.PlayerChangedHandler);
    }

    private static void ChangePlayerStats(Player playerToChange)
    {
        
        MenuButton[] playerStatsButtons = new MenuButton[playerToChange.Stats.Count];
        for (int i = 0; i < playerToChange.Stats.Count; i++)
            playerStatsButtons[i] = new ActionButton<int>(playerToChange.Stats[i].ToString(), index => playerToChange.Stats.RemoveAt(index), i);
        string[] statsActionsText = { "Добавить красную карточку", "Добавить жёлтую карточку", "Добавить гол", "Добавить голевую передачу", "Удалить статистику" };
        var statsButton = new SwapButton("Выберите действие со статистикой", statsActionsText);

        ButtonsGroup playerStatsGroup = new ButtonsGroup(){MenuButtons = playerStatsButtons};
        ButtonsGroup playerChangeStatsGroup = new ButtonsGroup(){MenuButtons = new MenuButton[]{statsButton}};
        var playerStatsMenu = new Menu(new[]{playerStatsGroup, playerChangeStatsGroup });
        playerStatsGroup.IsActive = false;
        playerStatsMenu.HandleUsing();
        
        switch (statsButton.CurVariant)
        {
            case "Удалить статистику":
                playerStatsGroup.IsActive = true;
                playerChangeStatsGroup.IsActive = false;
                playerStatsMenu.HandleUsing();
                break;
            case "Добавить красную карточку":
                playerToChange.Stats.Add(new Stat("Red Cards"));
                break;
            case "Добавить жёлтую карточку":
                playerToChange.Stats.Add(new Stat("Yellow Cards"));
                break;
            case "Добавить гол":
                playerToChange.Stats.Add(new Stat("Goals"));
                break;
            case "Добавить голевую передачу":
                playerToChange.Stats.Add(new Stat("Assists"));
                break;
        }
        
        playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now, playerToChange.Stats));
    }
    
    private static void Settings()
    {
        throw new NotImplementedException();
    }

    private static void Help()
    {
        throw new NotImplementedException();
    }
}