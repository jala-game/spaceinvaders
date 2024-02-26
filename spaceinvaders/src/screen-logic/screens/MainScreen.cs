using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model;
using spaceinvaders.model.barricades;
using spaceinvaders.model.sounds;
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
    private EMenuScreenOptions _selectedOption = EMenuScreenOptions.Play;
    private SpiralYellow spiralYellowAnimation = new SpiralYellow(spriteBatch, contentManager, new Vector2(20, 20));
    private float delayToPress = 10f;
    public override void Initialize() { }

    public override void LoadContent()
    {
        SoundEffects.LoadMusic(game, ESoundsEffects.BackgroundSongForMenu);
        SoundEffects.PlayEffects(true, 0.2f);
    }

    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();

        delayToPress--;
        if (delayToPress > 0) return;

        ModifyMenuSelection(kstate);
        SendMenuOption(kstate);
        spiralYellowAnimation.Update(gameTime);

    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawMenu();
        DrawDescription();
        DrawItemMenuActive();
        spiralYellowAnimation.Draw();
    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Up) && _selectedOption > 0)
        {
            _selectedOption--;
            delayToPress = resetDelay;
            PlayMenuSound(ESoundsEffects.MenuSelection);
        }
        else if (kstate.IsKeyDown(Keys.Down) && (int)_selectedOption < 2)
        {
            _selectedOption++;
            delayToPress = resetDelay;
            PlayMenuSound(ESoundsEffects.MenuSelection);
        }
        
    }

    private void PlayMenuSound(ESoundsEffects eSoundsEffects)
    {
        SoundEffects.LoadEffect(game,eSoundsEffects);
        SoundEffects.PlaySoundEffect(0.4f);
    }

    private void SendMenuOption(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        PlayMenuSound(ESoundsEffects.MenuEnter);
        switch (_selectedOption)
        {
            case EMenuScreenOptions.Play:
                SoundEffects.StopMusic();
                StartGame();
                break;
            case EMenuScreenOptions.Leaderboard:
                LeaderBoardsStart();
                break;
            case EMenuScreenOptions.Controls:
                ControlScreen();
                break;
        }
    }

    private void StartGame()
    {
        SpaceShip spaceShip = new(graphics, spriteBatch, contentManager, game);
        PlayScreen playScreen = new(game,spaceShip, graphics, contentManager, spriteBatch);
        ScreenManager.ChangeScreen(playScreen);
    }

    private void LeaderBoardsStart()
    {
        LeaderBoardsScreen leaderBoardsScreen = new LeaderBoardsScreen(game, graphics, contentManager, spriteBatch);
        ScreenManager.ChangeScreen(leaderBoardsScreen);
    }

    private void ControlScreen()
    {
        GameControlScreen GameControlScreen = new(game,graphics,contentManager, spriteBatch);
        ScreenManager.ChangeScreen(GameControlScreen);
    }

    private void DrawTitle()
    {
        string text = "Space Invaders";
        float textWidth = _gameFont.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameFont, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth ,100), Color.Gold);
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
            case EMenuScreenOptions.Play:
                DrawMenuItem("> Play Game", 400, Color.Red);
                break;
            case EMenuScreenOptions.Leaderboard:
                DrawMenuItem("> View Leaderboards", 500,Color.Red);
                break;
            case EMenuScreenOptions.Controls:
                DrawMenuItem("> Game Control", 600,Color.Red);
                break;
        }
    }

    private void DrawDescription()
    {
        string text = "Use the arrow keys to move between menu items and press enter to select the option.";
        spriteBatch.DrawString(_gameDescription, text, new Vector2(100,graphics.PreferredBackBufferHeight - 100), Color.White);
    }

}


