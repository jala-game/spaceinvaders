using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class SpiralYellow {
    private Animation _animation;
    private Vector2 _pos;

    public SpiralYellow(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        Texture2D a = content.Load<Texture2D>("effects/spiralyellow");
        _animation = new(spriteBatch, a, 5, 10, 10f, 2);
        _pos = pos;
        _animation.repeat = true;
    }

    public void Update(GameTime gameTime)
    {
        _animation.Update(gameTime);
    }

    public void Draw()
    {
        _animation.Draw(_pos);
    }
}