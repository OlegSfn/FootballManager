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

/// <summary>
/// Manages the routes and navigation within the application.
/// </summary>
public static class RouteManager
{
    /// <summary>
    /// Enters the main menu of the application.
    /// </summary>
    public static void EnterMainMenu()
    {
        var enterNewDataButton = new ActionButton("Ввести данные о новой игре из файла", EnterNewData);
        var menuButtons = new List<MenuButton>{enterNewDataButton};

        if (Storage.GameDatas.Count > 0)
        {
            var changeCurrentGameButton = new SwapButton<Game>("Текущая игра", Storage.GameDatas.ToArray(), swapAction:ChangeCurrentGame,
                startIndex: Storage.GameDatas.IndexOf(Storage.CurrentGame));
            var sortPlayersButton = new ActionButton("Отсортировать игроков", SortPlayers);
            var changePlayerDataButton = new ActionButton<Game>("Изменить данные об игроке", ChoosePlayerToChangePlayerData, Storage.CurrentGame);
            menuButtons.AddRange(new MenuButton[]{changeCurrentGameButton, sortPlayersButton, changePlayerDataButton});
        }
        
        var helpButton = new ActionButton("Помощь", OpenHelp);
        var exitButton = new ActionButton("Выход", () => Environment.Exit(0));
        menuButtons.AddRange(new MenuButton[]{helpButton, exitButton});
        var menu = new Menu(menuButtons.ToArray(), "Меню");
        menu.HandleUsing();
    }
    
    /// <summary>
    /// Opens the help menu providing guidance on how to use the application.
    /// </summary>
    public static void OpenHelp()
    {
        var movementsButton = new ActionButton("Чтобы передвигаться по меню, используйте стрелки вверх-вниз", Console.Beep);
        var actionButton = new ActionButton("Чтобы нажать на кнопку, используйте Enter", Console.Beep);
        var swapButton = new SwapButton<string>("Кнопки со стрелками можно переключать, используя стрелки вправо-влево, на них так же можно нажать с помощью Enter",
            new []{"1", "2", "3"}, confirmAction: _ => Console.Beep());
        var returnButton = new ActionButton("Чтобы вернуться на прошлую страницу, нажмите Tab", Console.Beep);

        var helpMenu = new Menu(new MenuButton[] { movementsButton, actionButton, swapButton, returnButton }, "Помощь");
        
        while (true)
        {
            if (!helpMenu.HandleUsing())
                return;
        }
        
    }


    private static void EnterNewData()
    {
        string? filePath = InputHandler.GetFilePathToJson("Введите путь до файла с данными: ");
        if (filePath == null)
            return;
        var players = GetPlayersFromFile(filePath);
        if (players == null || players.Count == 0)
            return;
        
        var gameData = new Game(players, Path.GetFileName(filePath));
        AddNewGame(gameData);
        Storage.CurrentGame = gameData;
    }

    private static List<Player>? GetPlayersFromFile(string filePath)
    {
        var players = new List<Player>();

        try
        {
            using var sr = new StreamReader(filePath);
            char firstLetter;
            while (char.IsWhiteSpace(firstLetter = (char)sr.Read()))
            {
            }

            using FileStream fs = File.OpenRead(filePath);
            if (firstLetter == '[')
                players = JsonSerializer.Deserialize<List<Player>>(fs);
            else if (firstLetter == '{')
                players = new List<Player> { JsonSerializer.Deserialize<Player>(fs)! };
            else
            {
                Printer.PrintError("Json файл должен начинаться с \"[\" или \"{\"");
                InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
                return players;
            }
        }
        catch (JsonException ex)
        {
            if (ex.InnerException is InvalidOperationException)
            {
                Printer.PrintError("В файле неверно указан номер игрока.");
                InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
                return players;
            }
            
            Printer.PrintError("Введён неверный json файл.");
            InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
            return players;
        }
        catch (ArgumentNullException)
        {
            Printer.PrintError("В файле есть некорректные поля или нет данных об игроках.");
            InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
            return players;
        }

        return players;
    }

    private static void ChangeCurrentGame(Game game)
    {
        Storage.CurrentGame = game;
    }

    private static void SortPlayers()
    {
        var sortTypes = new[] { "Id", "Имя", "Позиция", "Игровой номер", "Команда" };
        var sortModes = new[] { "По возрастанию", "По убыванию" };
        var sortTypeButton = new SwapButton<string>("Сортировать по", sortTypes, isCycled:true);
        var sortModeButton = new SwapButton<string>("Режим сортировки", sortModes, isCycled:true);
        var sortModeMenu = new Menu(new MenuButton[]{sortTypeButton, sortModeButton}, "Настройки сортировки");
        if (!sortModeMenu.HandleUsing())
            return;

        var isReversed = sortModeButton.CurVariant == "По убыванию";
        var sortedPlayers = Storage.CurrentGame.SortPlayers(sortTypeButton.CurVariant, isReversed);
        var newGameData = new Game(sortedPlayers.ToList(), $"{Storage.CurrentGame} (сортировка: {sortTypeButton.CurVariant} | {sortModeButton.CurVariant})");
        AddNewGame(newGameData);
        Storage.CurrentGame = newGameData;
        ChoosePlayerToChangePlayerData(newGameData);
    }

    private static void ChoosePlayerToChangePlayerData(Game game)
    {
        var playersTable = new Table(new[]{42, 30, 15, 15, 20}, AlignMode.Center, new[] {AlignMode.Center, AlignMode.Center, AlignMode.Center, AlignMode.Center, AlignMode.Center});
        var headerButton = new ActionButton(playersTable.FormatRowItems(new[] {"Id", "Name", "Jersey number", "Position", "Team"}));
        var choosePlayerHeaderButtonsGroup = new ButtonsGroup{MenuButtons = new MenuButton[]{headerButton}, IsActive = false};
        while (true)
        {
            Player chosenPlayer = null!;
            MenuButton[] playerButtons = game.Players.Select(
                    x => 
                        new ActionButton<Player>(playersTable.FormatRowItems(new[]{x.Id, x.Name, x.JerseyNumber.ToString(),
                            x.Position, x.TeamName}), player => chosenPlayer = player, x))
                .ToArray();
            
            var choosePlayerButtonsGroup = new ButtonsGroup{MenuButtons = playerButtons};
            var playersMenu = new Menu(new[]{choosePlayerHeaderButtonsGroup, choosePlayerButtonsGroup}, 
                "Выберите игрока, данные о котором вы хотите поменять");

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
            
            var changeFieldMenu = new Menu(new []{changePlayerMenuGroup}, $"Выберите параметр, который нужно изменить у {chosenPlayer.Name}");
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
        var positionsButton = new SwapButton<string>("Новая позиция", Storage.CurrentGame.Positions,
            startIndex: Array.FindIndex(Storage.CurrentGame.Positions, x => x == playerToChange.Position));
        var playerPositionButtonsGroup = new ButtonsGroup { MenuButtons = new MenuButton[]{ positionsButton} };
        var changePlayerPositionMenu = new Menu(new [] { playerDataButtonsGroup, playerPositionButtonsGroup },
            $"Выберите параметр, который нужно изменить у {playerToChange.Name}");
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
        var teamsButton = new SwapButton<string>("Новая команда", teamsNames, 
            startIndex: Storage.CurrentGame.Teams.FindIndex(x => x.Name == playerToChange.TeamName)); 
        var playerTeamButtonsGroup = new ButtonsGroup { MenuButtons = new MenuButton[]{ teamsButton} };
        var changePlayerTeamMenu = new Menu(new[] { playerDataButtonsGroup, playerTeamButtonsGroup }, 
            $"Выберите параметр, который нужно изменить у {playerToChange.Name}");
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
        var statsTable = new Table(new[]{42, 20}, AlignMode.Center, new[] {AlignMode.Center, AlignMode.Center});
        
        var headerButton = new ActionButton(statsTable.FormatRowItems(new[] {"Id", "Type"}));
        var playerstatsHeaderButtonsGroup = new ButtonsGroup{MenuButtons = new MenuButton[]{headerButton}, IsActive = false};
        
        while (true)
        {
            MenuButton[] playerStatsButtons = new MenuButton[playerToChange.Stats.Count];
            for (int i = 0; i < playerToChange.Stats.Count; i++)
                playerStatsButtons[i] = new ActionButton<int>(statsTable.FormatRowItems(new [] {playerToChange.Stats[i].Id, playerToChange.Stats[i].Type}),
                    index =>
                    {
                        playerToChange.Stats.RemoveAt(index);
                        playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
                    }, i);

            var playerStatsGroup = new ButtonsGroup{MenuButtons = playerStatsButtons, IsActive = false};
            var playerStatsMenu = new Menu(new[]{playerstatsHeaderButtonsGroup, playerStatsGroup, playerChangeStatsGroup });
            if (!playerStatsMenu.HandleUsing())
                return;
        
            switch (statsButton.CurVariant)
            {
                case "Удалить статистику":
                    if (playerToChange.Stats.Count == 0)
                    {
                        Printer.PrintError("У игрока нет статистики.");
                        InputHandler.WaitForUserInput("Нажмите любую клавишу для продолжения: "); 
                        break;
                    }
                        
                    playerStatsGroup.IsActive = true;
                    playerChangeStatsGroup.IsActive = false;
                    playerStatsMenu.HandleUsing();
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
    
    private static void GameChangedHandler(object? sender, GameUpdatedEventArgs e)
    {
        Printer.PrintInfo($"Сообщение от игры: {e.Message}");
        InputHandler.WaitForUserInput("Нажмите любую клавишу, чтобы продолжить: ");
    }

    private static void AddNewGame(Game newGame)
    {
        var autoSaver = new AutoSaver();
        Storage.GameDatas.Add(newGame);
        newGame.AttachObserverToAll(autoSaver.PlayerChangedHandler);
        newGame.AttachObserver(GameChangedHandler);
    }
}