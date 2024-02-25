using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model.barricades;
using spaceinvaders.model.sounds;
using spaceinvaders.services;

public class GameOverScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch,
    int score
    ) : GameScreenModel
{

    private SpriteFont _gameOverFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private SpriteFont _gameScoreFont = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
    private SpriteFont _gameMenuFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private EMenuOptionsGameOver _selectedOption = EMenuOptionsGameOver.SaveGame;
    private float delayToPress = 10f;

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
    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Up) && _selectedOption > EMenuOptionsGameOver.SaveGame)
        {
            _selectedOption--;
            delayToPress = resetDelay;
            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }

        if (kstate.IsKeyDown(Keys.Down) && _selectedOption < EMenuOptionsGameOver.LeaveGame)
        {
            _selectedOption++;
            delayToPress = resetDelay;
            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }
    }

    private void SendMenuOption(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        PlaySoundEffect(ESoundsEffects.MenuEnter);
        switch (_selectedOption)
        {
            case EMenuOptionsGameOver.SaveGame:
                SwitchToSaveScoreScreen();
                break;
            case EMenuOptionsGameOver.LeaveGame:
                LeaveTheGame();
                break;

        }
    }

    private void SwitchToSaveScoreScreen() {
        SaveScoreScreen saveScoreScreen = new(game, graphics, contentManager, spriteBatch, score);
        ScreenManager.ChangeScreen(saveScoreScreen);
    }

    private void LeaveTheGame()
    {
        SoundEffects.StopMusic();
        MainScreen mainScreen = new MainScreen(game,graphics,contentManager,spriteBatch );
        ScreenManager.ChangeScreen(mainScreen);
    }

    public override void Draw(GameTime gameTime) {
        DrawGameOver();
        DrawScore();
        DrawMenu();
        DrawItemMenuActive();
    }

    private void DrawGameOver() {
        string text = "GAME OVER";
        float textWidth = _gameOverFont.MeasureString(text).X / 2;
        float textHeight = _gameOverFont.MeasureString(text).Y / 2;
        spriteBatch.DrawString(_gameOverFont, text, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            graphics.PreferredBackBufferHeight / 2 - textHeight),
            Color.White);
    }

    private void DrawScore() {
        string text = $"TOTAL SCORE: {score}";
        float textWidth = _gameScoreFont.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameScoreFont, text, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            graphics.PreferredBackBufferHeight / 2 + 50),
            Color.Green);
    }

    private void DrawMenu()
    {
        List<string> stringsMenu = new List<string>() {"  Save Game","  Leave the game",};
        int baseY = graphics.PreferredBackBufferHeight / 2 + 150;

        stringsMenu.ForEach(e =>
        {
            DrawMenuItem(e, baseY, null);
            baseY += 100;
        });
    }

    private void DrawMenuItem(string text, int y, Color? color)
    {
        string textBase = "  Save Game";
        float textWidthItem = _gameMenuFont.MeasureString(textBase).X / 2 + 45;
        spriteBatch.DrawString(_gameMenuFont, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidthItem ,y), color ?? Color.White);
    }

    private void DrawItemMenuActive()
    {
        switch (_selectedOption)
        {
            case EMenuOptionsGameOver.SaveGame:
                DrawMenuItem("> Save Game", graphics.PreferredBackBufferHeight / 2 + 150, Color.Red);
                break;
            case EMenuOptionsGameOver.LeaveGame:
                DrawMenuItem("> Leave the game", graphics.PreferredBackBufferHeight / 2 + 250,Color.Red);
                break;
        }
    }
    
    private void PlaySoundEffect(ESoundsEffects effects)
    {
        SoundEffects.LoadEffect(game, effects);
        SoundEffects.PlaySoundEffect(0.4f);
    }
}