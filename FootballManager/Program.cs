namespace FootballManager;
public class Program
{
    public static void Main()
    {
        RouteManager.OpenHelp();
        
        while (true)
        {
            RouteManager.EnterMainMenu();
        }
    }
}