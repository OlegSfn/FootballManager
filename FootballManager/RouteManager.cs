using System.Text.Json;
using BusinessLogic;
using BusinessLogic.EventArgs;
using BusinessLogic.PlayerData;
using UILayer.MenuClasses.MenuButtonsClasses;
using DataLayer;
using UILayer;
using UILayer.MenuClasses;
using UILayer.MenuClasses.MenuButtonGroupsClasses;
using UILayer.TableClasses;

namespace FootballManager;

public static class RouteManager
{
    public static void HandleFirstUsing()
    {
        
    }
    
    public static void EnterMainMenu()
    {
        var enterNewDataButton = new ActionButton("Ввести данные о новой игре из файла", EnterNewData);
        List<MenuButton> menuButtons = new List<MenuButton>{enterNewDataButton};

        if (Storage.GameDatas.Count > 0)
        {
            var changeCurrentGameButton = new SwapButton<GameData>("Текущая игра", Storage.GameDatas.ToArray(), swapAction:ChangeCurrentGame);
            var sortPlayersButton = new ActionButton("Отсортировать игроков", SortPlayers);
            var changePlayerDataButton = new ActionButton<GameData>("Изменить данные об игроке", ChoosePlayerToChangePlayerData, Storage.CurrentGame);
            menuButtons.AddRange(new MenuButton[]{changeCurrentGameButton, sortPlayersButton, changePlayerDataButton});
        }
        
        var settingsButton = new ActionButton("Настройки", Settings);
        var helpButton = new ActionButton("Помощь", Help);
        var exitButton = new ActionButton("Выход", () => Environment.Exit(0));
        menuButtons.AddRange(new MenuButton[]{settingsButton, helpButton, exitButton});
        var menu = new Menu("Меню:", menuButtons.ToArray());
        menu.HandleUsing();
    }

    private static void EnterNewData()
    {
        string? filePath = InputHandler.GetFilePathToJson("Введите путь до файла с данными: ");
        if (filePath == null)
            return;

        List<Player> players = new List<Player>();
        //TODO: make another check on array.
        try
        {
            players = JsonSerializer.Deserialize<List<Player>>(File.ReadAllText(filePath));
        }
        catch (JsonException)
        {
            try
            {
                players = new List<Player> { JsonSerializer.Deserialize<Player>(File.ReadAllText(filePath)) };
            }
            catch (JsonException)
            {
                Printer.PrintError("Введён неверный json файл.");
                InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
            }
        }
        
        if (players != null)
        {
            var gameData = new GameData(players, Path.GetFileName(filePath));
            Storage.GameDatas.Add(gameData);
            Storage.CurrentGame = gameData;
            AddNewGame(gameData);
        }
        else
            Console.WriteLine("Не найдены данные об игроках в файле.");
        
    }

    private static void ChangeCurrentGame(GameData game)
    {
        Storage.CurrentGame = game;
    }

    private static void SortPlayers()
    {
        var sortTypes = new[] { "Id", "Имя", "Позиция", "Игровой номер", "Команда" };
        var sortModes = new[] { "По возрастанию", "По убыванию" };
        var sortTypeButton = new SwapButton<string>("Сортировать по", sortTypes, isCycled:true);
        var sortModeButton = new SwapButton<string>("Режим сортировки", sortModes, isCycled:true);
        var sortModeMenu = new Menu("Настройки сортировки:",new MenuButton[]{sortTypeButton, sortModeButton});
        if (!sortModeMenu.HandleUsing())
            return;

        bool isReversed = sortModeButton.CurVariant == "По убыванию";
        var sortedPlayers = Storage.CurrentGame.SortPlayers(sortTypeButton.CurVariant, isReversed);
        var newGameData = new GameData(sortedPlayers.ToList(), $"{Storage.CurrentGame} (сортировка: {sortTypeButton.CurVariant} | {sortModeButton.CurVariant})");
        Storage.GameDatas.Add(newGameData);
        AddNewGame(newGameData);
        ChoosePlayerToChangePlayerData(newGameData);
    }

    private static void ChoosePlayerToChangePlayerData(GameData gameData)
    {
        var playersTable = new Table(new[]{42, 30, 15, 15, 20}, AlignMode.Center, new[] {AlignMode.Center, AlignMode.Center, AlignMode.Center, AlignMode.Center, AlignMode.Center});
        var headerButton = new ActionButton(playersTable.FormatRowItems(new[] {"Id", "Name", "Jersey number", "Position", "Team"}));
        var choosePlayerHeaderButtonsGroup = new ButtonsGroup{MenuButtons = new MenuButton[]{headerButton}, IsActive = false};
        while (true)
        {
            Player chosenPlayer = null!;
            MenuButton[] playerButtons = gameData.Players.Select(
                    x => 
                        new ActionButton<Player>(playersTable.FormatRowItems(new[]{x.Id, x.Name, x.JerseyNumber.ToString(),
                            x.Position, x.TeamName}), player => chosenPlayer = player, x))
                .ToArray();
            
            var choosePlayerButtonsGroup = new ButtonsGroup{MenuButtons = playerButtons};
            var playersMenu = new Menu("Выберите игрока, данные о котором вы хотите поменять:", new[]{choosePlayerHeaderButtonsGroup, choosePlayerButtonsGroup});

            if (!playersMenu.HandleUsing())
                return;
            
            ChangePlayerData(chosenPlayer);
        }
    }

    private static void ChangePlayerData(Player chosenPlayer)
    {
        var lastCursorPosition = 0;
        while (true)
        {
            var changePlayerMenuGroup = new ButtonsGroup {CursorPosition = lastCursorPosition};
            var changeNameButton = new ActionButton($"Имя ({chosenPlayer.Name})", () => ChangePlayerName(chosenPlayer));
            var changePositionButton = new ActionButton($"Позиция ({chosenPlayer.Position})", () => ChangePlayerPosition(chosenPlayer, changePlayerMenuGroup));
            var changeJerseyNumButton = new ActionButton($"Игровой номер ({chosenPlayer.JerseyNumber})", () => ChangePlayerJerseyNumber(chosenPlayer));
            var changeTeamButton = new ActionButton($"Команда ({chosenPlayer.TeamName})", () => ChangePlayerTeam(chosenPlayer, changePlayerMenuGroup));
            var changeStatsButton = new ActionButton("Статистика", () => ChangePlayerStats(chosenPlayer));
            MenuButton[] changePlayerButtons =
                { changeNameButton, changePositionButton, changeJerseyNumButton, changeTeamButton, changeStatsButton };
            changePlayerMenuGroup.MenuButtons = changePlayerButtons;
            
            var changeFieldMenu = new Menu($"Выберите параметр, который нужно изменить у {chosenPlayer.Name}:", new []{changePlayerMenuGroup});
            if (!changeFieldMenu.HandleUsing())
                break;
            lastCursorPosition = changePlayerMenuGroup.CursorPosition;
        }
    }

    private static void ChangePlayerName(Player playerToChange)
    {
        Console.Write("Введите новое имя: ");
        var newName = Console.ReadLine();
        if (newName == null)
            return;
        playerToChange.Name = newName;
    }

    private static void ChangePlayerPosition(Player playerToChange, ButtonsGroup playerDataButtonsGroup)
    {
        playerDataButtonsGroup.IsActive = false;
        var positionsButton = new SwapButton<string>("Новая позиция", Storage.CurrentGame.Positions);
        var playerPositionButtonsGroup = new ButtonsGroup { MenuButtons = new MenuButton[]{ positionsButton} };
        var changePlayerPositionMenu = new Menu($"Выберите параметр, который нужно изменить у {playerToChange.Name}:", new [] { playerDataButtonsGroup, playerPositionButtonsGroup });
        if (!changePlayerPositionMenu.HandleUsing())
        {
            playerDataButtonsGroup.IsActive = true;
            return;
        }
        
        playerToChange.Position = positionsButton.CurVariant;
        playerDataButtonsGroup.IsActive = true;
    }
    
    private static void ChangePlayerJerseyNumber(Player playerToChange)
    {
        while (true)
        {
            Console.Write("Введите новый номер: ");
            if (!InputHandler.GetIntValue("Введите целое число для номера: ", "Введите новое целое число для номера: ",
                    out int newJerseyNumber)) continue;
            
            if (newJerseyNumber >= 0)
            {
                Printer.PrintError("Число должно быть неотрицательным.");
                continue;
            }
            playerToChange.JerseyNumber = newJerseyNumber;
            return;
        }
    }

    private static void ChangePlayerTeam(Player playerToChange, ButtonsGroup playerDataButtonsGroup)
    {
        playerDataButtonsGroup.IsActive = false;
        string[] teamsNames = Storage.CurrentGame.Teams.Select(x => x.Name).Append("Создать новую команду").ToArray();
        var teamsButton = new SwapButton<string>("Новая команда", teamsNames); 
        var playerTeamButtonsGroup = new ButtonsGroup { MenuButtons = new MenuButton[]{ teamsButton} };
        var changePlayerTeamMenu = new Menu($"Выберите параметр, который нужно изменить у {playerToChange.Name}:",new[] { playerDataButtonsGroup, playerTeamButtonsGroup });
        if (!changePlayerTeamMenu.HandleUsing())
        {
            playerDataButtonsGroup.IsActive = true;
            return;
        }

        var newTeamName = teamsButton.CurVariant;
        if (newTeamName == "Создать новую команду")
        {
            Console.Write("Введите название для новой команды: ");
            newTeamName = Console.ReadLine();
            if (newTeamName == null)
                return;
            
            if (Storage.CurrentGame.Teams.All(x => x.Name != newTeamName))
                Storage.CurrentGame.CreateNewTeam(newTeamName);
        }
        
        playerToChange.DetachObserver(playerToChange.Team.PlayerChangedHandler);
        var newTeam = Storage.CurrentGame.Teams.First(x => x.Name == newTeamName);
        playerToChange.Team = newTeam;
        playerToChange.TeamName = newTeamName;
        playerToChange.AttachObserver(newTeam.PlayerChangedHandler);
        playerDataButtonsGroup.IsActive = true;
    }

    private static void ChangePlayerStats(Player playerToChange)
    {
        string[] statsActionsText = { "Добавить красную карточку", "Добавить жёлтую карточку", "Добавить гол", "Добавить голевую передачу", "Удалить статистику" };
        var statsButton = new SwapButton<string>("Выберите действие со статистикой", statsActionsText);
        var playerChangeStatsGroup = new ButtonsGroup{MenuButtons = new MenuButton[]{statsButton}};
        while (true)
        {
            MenuButton[] playerStatsButtons = new MenuButton[playerToChange.Stats.Count];
            for (int i = 0; i < playerToChange.Stats.Count; i++)
                playerStatsButtons[i] = new ActionButton<int>(playerToChange.Stats[i].ToString(), index => playerToChange.Stats.RemoveAt(index), i);

            var playerStatsGroup = new ButtonsGroup{MenuButtons = playerStatsButtons, IsActive = false};
            var playerStatsMenu = new Menu(new[]{playerStatsGroup, playerChangeStatsGroup });
            if (!playerStatsMenu.HandleUsing())
                return;
        
            switch (statsButton.CurVariant)
            {
                case "Удалить статистику":
                    if (playerToChange.Stats.Count == 0)
                    {
                        Printer.PrintError("У игрока нет статистики.");
                        InputHandler.WaitForUserInput("Нажмите любую клавишу для продолжения: "); //TODO: mb change? 
                        break;
                    }
                        
                    playerStatsGroup.IsActive = true;
                    playerChangeStatsGroup.IsActive = false;
                    playerStatsMenu.HandleUsing();
                    playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    playerChangeStatsGroup.IsActive = true;
                    break;
                case "Добавить красную карточку":
                    playerToChange.Stats.Add(new Stat("Red Cards"));
                    playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    break;
                case "Добавить жёлтую карточку":
                    playerToChange.Stats.Add(new Stat("Yellow Cards"));
                    playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    break;
                case "Добавить гол":
                    playerToChange.Stats.Add(new Stat("Goals"));
                    playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    break;
                case "Добавить голевую передачу":
                    playerToChange.Stats.Add(new Stat("Assists"));
                    playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    break;
            }
        }
    }
    
    private static void Settings()
    {
        throw new NotImplementedException();
    }

    private static void Help()
    {
        throw new NotImplementedException();
    }

    private static void GameChangedHandler(object? sender, GameUpdatedEventArgs e)
    {
        Printer.PrintInfo(e.Message);
        InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
    }

    private static void AddNewGame(GameData newGameData)
    {
        var autoSaver = new AutoSaver();
        newGameData.AttachObserverToAll(autoSaver.PlayerChangedHandler);
        newGameData.AttachObserver(GameChangedHandler);
    }
}