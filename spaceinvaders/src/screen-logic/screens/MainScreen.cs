using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class MainScreen(
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch
) : GameScreenModel
{
    private SpriteFont _gameFont;
    private SpriteFont _gameFontSmall;
    private int _selectedOption = 1;
    public override void Initialize() { }

    public override void LoadContent()
    {
        _gameFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        _gameFontSmall = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawMenu();
        
    }

    private void DrawTitle()
    {
        string text = "Space Invaders";
        float textWidth = _gameFont.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameFont, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth ,100), Color.White);
    }

    private void DrawMenu()
    {
        string textItem1 = "Play Game";
        float textWidthItem1 = _gameFontSmall.MeasureString(textItem1).X / 2;
        spriteBatch.DrawString(_gameFontSmall, textItem1, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidthItem1 ,300), Color.White);
        
        string textItem2 = "> Play Game";
        float textWidthItem2 = _gameFontSmall.MeasureString(textItem1).X / 2;
        spriteBatch.DrawString(_gameFontSmall, textItem2, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidthItem2 ,360), Color.White);
    }
    
    
}


