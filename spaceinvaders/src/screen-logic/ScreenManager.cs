public class ScreenManager
{
    private static GameScreenModel currentScreen;

    public static void ChangeScreen(GameScreenModel newScreen)
    {
        currentScreen = newScreen;
        currentScreen.LoadContent();
    }

    public static void Update()
    {
        currentScreen.Update();
    }

    public static void Draw()
    {
        currentScreen.Draw();
    }
}