using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using spaceinvaders.model.barricades;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch) : GameScreenModel
{
    private SpriteFont _title = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private SpriteFont _genericFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private int _page = 0;
    private EMenuOptionsLeaderBoards _chooseMenu = EMenuOptionsLeaderBoards.RightArrow;
    private float delayToPress = 10f;
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    private void DrawControls()
    {
        DrawTitle("Controls for Ship", null);
    }

    private void DrawGameScore()
    {
        DrawTitle("Game Score", Color.Gold);
    }
    

    private void DrawTitle(string text, Color? color)
    {
        float textWidth = _title.MeasureString(text).X / 2;
        spriteBatch.DrawString(_title, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth, 50),
            color ?? Color.White);
    }
    
    
}