using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model;

public class Score
{
    private int _score;
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _spriteFont;
    private readonly ContentManager _contentManager;

    public Score(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager)
    {
        _graphics = graphics;
        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
        _spriteFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
        _score = 0;
    }

    public int GetScore()
    {
        return _score;
    }

    public void SetScore(int points)
    {
        if (points < 0) return;

        _score += points;
    }

    public void Draw()
    {
        _spriteBatch.DrawString(_spriteFont, $"SCORE {_score}", new Vector2(_graphics.PreferredBackBufferWidth - 250, 50), Color.White);
    }
}