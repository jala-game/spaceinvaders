using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.screen_logic.screens;

public class MainScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch
) : GameScreenModel
{
    private SpriteFont _gameFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private SpriteFont _gameFontSmall = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private SpriteFont _gameDescription = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
    private int _selectedOption = 1;
    private float delayToPress = 10f;
    public override void Initialize() { }

    public override void LoadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();

        delayToPress--;
        if (delayToPress > 0) return;

        ModifyMenuSelection(kstate);
        SendMenuOption(kstate);

    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawMenu();
        DrawDescription();
        DrawItemMenuActive();

    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Up) && _selectedOption > 1)
        {
            _selectedOption--;
            delayToPress = resetDelay;
        }

        if (kstate.IsKeyDown(Keys.Down) && _selectedOption < 3)
        {
            _selectedOption++;
            delayToPress = resetDelay;
        }
    }

    private void SendMenuOption(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        switch (_selectedOption)
        {
            case 1:
                StartGame();
                break;
            case 2:
                break;
            case 3:
                ControlScreen();
                break;
        }
    }

    private void DrawTitle()
    {
        string text = "Space Invaders";
        float textWidth = _gameFont.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameFont, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth ,100), Color.White);
    }

    private void DrawMenu()
    {
        List<string> stringsMenu = new List<string>() {"  Play Game","  View Leaderboards","  Game Control" };
        int baseY = 400;

        stringsMenu.ForEach(e =>
        {
            DrawMenuItem(e, baseY, null);
            baseY += 100;
        });
    }

    private void StartGame()
    {
        SpaceShip spaceShip = new(graphics, spriteBatch, contentManager);
        PlayScreen playScreen = new(game,spaceShip, graphics, contentManager, spriteBatch);
        ScreenManager.ChangeScreen(playScreen);
    }

    private void ControlScreen()
    {
        GameControlScreen gameControlScreen = new(game);
        ScreenManager.ChangeScreen(gameControlScreen);
    }

    private void DrawMenuItem(string text, int y, Color? color)
    {
        string textBase = "  Play Game";
        float textWidthItem1 = _gameFontSmall.MeasureString(textBase).X / 2 + 45;
        spriteBatch.DrawString(_gameFontSmall, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidthItem1 ,y),color ?? Color.White);
    }

    private void DrawItemMenuActive()
    {
        switch (_selectedOption)
        {
            case 1:
                DrawMenuItem("> Play Game", 400, Color.Green);
                break;
            case 2:
                DrawMenuItem("> View Leaderboards", 500,Color.Green);
                break;
            case 3:
                DrawMenuItem("> Game Control", 600,Color.Green);
                break;
        }
    }

    private void DrawDescription()
    {
        string text = "Use the arrow keys to move between menu items and press enter to select the option.";
        float textWidth = _gameDescription.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameDescription, text, new Vector2(100,graphics.PreferredBackBufferHeight - 100), Color.White);
    }

}


