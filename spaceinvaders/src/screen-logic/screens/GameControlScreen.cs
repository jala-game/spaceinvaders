using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using spaceinvaders.model.barricades;
using spaceinvaders.model.sounds;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch) : GameScreenModel
{
    private SpriteFont _title = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private SpriteFont _genericFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private int _page = 1;
    private int _limitedPages = 2;
    private EMenuOptionsLeaderBoards _chooseMenu = EMenuOptionsLeaderBoards.RightArrow;
    private float delayToPress = 10f;
    

    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();
        
        delayToPress--;
        if (delayToPress > 0) return;
        
        ModifyMenuSelection(kstate);
        SendMenuOption(kstate);
        base.Update(gameTime);
    }

    private void SendMenuOption(KeyboardState kstate)
    {
        delayToPress--;
        if (delayToPress > 0) return;
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        PlaySoundEffect(ESoundsEffects.MenuEnter);
        switch (_chooseMenu)
        {
            case EMenuOptionsLeaderBoards.LeaveGame:
                LeaveTheGame();
                break;
            case EMenuOptionsLeaderBoards.LeftArrow:
                ReturnPages();
                delayToPress = 10;
                break;
            case EMenuOptionsLeaderBoards.RightArrow:
                NextPages();
                delayToPress = 10;
                break;
        }
    }
    
    private void LeaveTheGame()
    {
        MainScreen mainScreen = new MainScreen(game,graphics,contentManager,spriteBatch );
        ScreenManager.ChangeScreen(mainScreen);
    }
    
    private void ReturnPages()
    {
        if (_page <= 1) return;
        _page--;

    }
    
    private void NextPages()
    {
        if (_page >= _limitedPages) return;
        _page++;
    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Left) && _chooseMenu > EMenuOptionsLeaderBoards.LeaveGame)
        {
            _chooseMenu--;
            delayToPress = resetDelay;
            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }
        
        if (kstate.IsKeyDown(Keys.Right) && _chooseMenu < EMenuOptionsLeaderBoards.RightArrow)
        {
            _chooseMenu++;
            delayToPress = resetDelay;
            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        switch (_page)
        {
            case 1:
                DrawControls();
                break;
            case 2:
                DrawGameScore();
                break;
        }
        DrawMenu();
        DrawMenuItemActive();
        base.Draw(gameTime);
    }

    private void DrawMenuItemActive()
    {
        switch (_chooseMenu)
        {
            case  EMenuOptionsLeaderBoards.LeaveGame:
                DrawMenuItem(100, "> Back", Color.Green);
                break;
            case EMenuOptionsLeaderBoards.RightArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($">").X / 2) + 70, ">", Color.Green);
                break;
            case EMenuOptionsLeaderBoards.LeftArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"<").X / 2) - 70, "<", Color.Green);
                break;
        }
    }

    private void DrawMenu()
    {
        string[] texts = new[] { "  Back" , "<",$"{_page}",">"};
        float[] positionsX = new[] { 100, 
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"<").X / 2) - 70,
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"{_page + 1}").X / 2),
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($">").X / 2) + 70,
        };

        for (int i = 0; i < texts.Length; i++)
        {
            DrawMenuItem(positionsX[i],texts[i], null);
        }
    }
    private void DrawMenuItem(float x, string text, Color? color)
    {
        spriteBatch.DrawString(_genericFont,text,new Vector2(x,graphics.PreferredBackBufferHeight - 100),color ?? Color.White);
    }

    private void DrawControls()
    {
        DrawTitle("Controls for Ship", null);
        DrawKeyControls();
    }

    private void DrawGameScore()
    {
        DrawTitle("Game Score", Color.Gold);
        DrawEnemyScore();
    }

    private void DrawKeyControls()
    {

        string[] controls = new[] {"Move to the left - Left arrow or A Key",
            "Move to the right - Right arrow or D Key","Shoot - Space Key" };
        int positionY = 300;

        for (int i = 0; i < controls.Length; i++)
        {
            spriteBatch.DrawString(_genericFont, controls[i], new Vector2(300,positionY), Color.White);
            positionY += 100;
        }
        
    }
    private void DrawEnemyScore()
    {
        Texture2D[] enemys = new[]
        {
            contentManager.Load<Texture2D>("aliens/front-alien-ship"),
            contentManager.Load<Texture2D>("aliens/bird-alien-ship"),
            contentManager.Load<Texture2D>("aliens/shooter-alien-ship"),
            contentManager.Load<Texture2D>("aliens/red-alien-ship")
        };

        string[] points = new[] { " = 10 PTS", " = 20 PTS", " = 40 PTS", " = ??? PTS" };

        int positionY = 300;

        for (int i = 0; i < enemys.Length; i++)
        {
            float textWidth = _title.MeasureString("Game Score").X / 2 - 30;
            spriteBatch.Draw(enemys[i], new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth,positionY), Color.White);
            spriteBatch.DrawString(_genericFont, points[i], new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth + 100, positionY), Color.White);
            positionY += 100;
        }



    }
    
    private void DrawTitle(string text, Color? color)
    {
        float textWidth = _title.MeasureString(text).X / 2;
        spriteBatch.DrawString(_title, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth, 50),
            color ?? Color.White);
    }
    
    private void PlaySoundEffect(ESoundsEffects effects)
    {
        SoundEffects.LoadEffect(game, effects);
        SoundEffects.PlaySoundEffect(0.4f);
    }
    
}