using Microsoft.Xna.Framework;
using spaceinvaders.screen_logic.screens;

public abstract class ScreenManager
{
    private static GameScreenModel _currentScreen;

    public static void ChangeScreen(GameScreenModel newScreen)
    {
        _currentScreen = newScreen;
        _currentScreen.LoadContent();
    }

    public static void Initialize()
    {
        _currentScreen.Initialize();
    }

    public static void Update(GameTime gameTime)
    {
        _currentScreen.Update(gameTime);
    }

    public static void Draw(GameTime gameTime)
    {
        _currentScreen.Draw(gameTime);
    }
}