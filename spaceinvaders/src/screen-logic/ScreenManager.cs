using Microsoft.Xna.Framework;

public class ScreenManager
{
    private static GameScreenModel currentScreen;

    public static void ChangeScreen(GameScreenModel newScreen)
    {
        currentScreen = newScreen;
        currentScreen.LoadContent();
    }

    public static void Initialize() {
        currentScreen.Initialize();
    }

    public static void Update(GameTime gameTime)
    {
        currentScreen.Update(gameTime);
    }

    public static void Draw(GameTime gameTime)
    {
        currentScreen.Draw(gameTime);
    }
}
