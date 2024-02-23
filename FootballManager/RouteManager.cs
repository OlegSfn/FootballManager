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
            var changePlayerDataButton = new ActionButton<GameData>("Изменить данные об игроке", ChosePlayerToChangePlayerData, Storage.CurrentGame);
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
        ChosePlayerToChangePlayerData(newGameData);
    }

    private static void ChosePlayerToChangePlayerData(GameData gameData)
    {
        while (true)
        {
            Player chosenPlayer = null!;
            MenuButton[] playerButtons = gameData.Players.Select(
                    x => 
                        new ActionButton<Player>($"{x.Id} | {x.Name} | {x.TeamName} | {x.JerseyNumber}", player => chosenPlayer = player, x))
                .ToArray();
            var playersMenu = new Menu("Выберите игрока, данные о котором вы хотите поменять: ", playerButtons);

            if (!playersMenu.HandleUsing())
                return;
            
            ChangePlayerData(chosenPlayer);
        }
    }

    private static void ChangePlayerData(Player chosenPlayer)
    {
        ButtonsGroup changePlayerVariableButtonsGroup = new()
        {
            MenuButtons = new MenuButton[] {new ActionButton(""){IsActive = false}}, //TODO: check error on empty.
            IsActive = false
        };
        ButtonsGroup changePlayerButtonsGroup = new();
        void SwitchGroup(bool isBaseGroupActive)
        {
            changePlayerVariableButtonsGroup.IsActive = !isBaseGroupActive;
            changePlayerVariableButtonsGroup.IsVisible = !isBaseGroupActive;
            changePlayerButtonsGroup.IsActive = isBaseGroupActive;
        }    
        
        
        var changeNameButton = new ActionButton("Имя", () => ChangePlayerName(chosenPlayer));
        var changePositionButton = new ActionButton("Позиция", () =>
        {
            var positionsButton = new SwapButton<string>("Новая позиция", Storage.CurrentGame.Positions,
                confirmAction:(newPosition) =>
                {
                    ChangePlayerPosition(chosenPlayer, newPosition);
                    SwitchGroup(true);
                });
            changePlayerVariableButtonsGroup.MenuButtons[0] = positionsButton;
            SwitchGroup(false);
        });
        var changeJerseyNumButton = new ActionButton("Игровой номер", () => ChangePlayerJerseyNumber(chosenPlayer));
        var changeTeamButton = new ActionButton("Команда", () =>
        {
            string[] teamsNames = Storage.CurrentGame.Teams.Select(x => x.Name).Append("Создать новую команду").ToArray();
            var teamsButton = new SwapButton<string>("Новая команда", teamsNames, 
                confirmAction:(newTeam) =>
                {
                    ChangePlayerTeam(chosenPlayer, newTeam);
                    SwitchGroup(true);
                });
            changePlayerVariableButtonsGroup.MenuButtons[0] = teamsButton;
            SwitchGroup(false);
        });
        var changeStatsButton = new ActionButton("Статистика", () => ChangePlayerStats(chosenPlayer));
        MenuButton[] changePlayerButtons =
            { changeNameButton, changePositionButton, changeJerseyNumButton, changeTeamButton, changeStatsButton };
            
        changePlayerButtonsGroup.MenuButtons = changePlayerButtons;
            
        while (true)
        {
            var changeFieldMenu = new Menu($"Выберите параметр, который нужно изменить у {chosenPlayer.Name}:",
                new []{changePlayerButtonsGroup, changePlayerVariableButtonsGroup});
            if (!changeFieldMenu.HandleUsing())
                break;
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

    private static void ChangePlayerPosition(Player playerToChange, string newPosition)
    {
        playerToChange.Position = newPosition;
    }
    
    private static void ChangePlayerJerseyNumber(Player playerToChange)
    {
        Console.Write("Введите новый номер: ");
        if(InputHandler.GetIntValue("Введите целое число для номера: ", "Введите новое целое число для номера: ", out int newJerseyNumber))
            playerToChange.JerseyNumber = newJerseyNumber;
    }

    private static void ChangePlayerTeam(Player playerToChange, string newTeamName)
    {
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
        
            playerToChange.OnPlayerUpdated(new PlayerUpdatedEventArgs(DateTime.Now));
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