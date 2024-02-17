using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class GameOverScreen(
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch,
    int score
    ) : GameScreenModel {

    private SpriteFont _gameOverFont;
    private SpriteFont _gameScoreFont;
    public override void Initialize() { }
    public override void LoadContent() { 
        _gameOverFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        _gameScoreFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
    }
    public override void Update(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) {
        DrawGameOver();
        DrawScore();
    }

    public void DrawGameOver() {
        string text = "GAME OVER";
        float textWidth = _gameOverFont.MeasureString(text).X / 2;
        float textHeight = _gameOverFont.MeasureString(text).Y / 2;
        spriteBatch.DrawString(_gameOverFont, text, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            graphics.PreferredBackBufferHeight / 2 - textHeight),
            Color.White);
    }

    public void DrawScore() {
        string text = $"TOTAL SCORE: {score}";
        float textWidth = _gameScoreFont.MeasureString(text).X / 2;
        spriteBatch.DrawString(_gameScoreFont, text, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            graphics.PreferredBackBufferHeight / 2 + 50),
            Color.Green);
    }
}