using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Explosion {
    private SpriteBatch _spriteBatch;
    private Animation _animation;

    public Explosion(SpriteBatch spriteBatch, ContentManager content)
    {
        Texture2D a = content.Load<Texture2D>("effects/explosion");
        _animation = new(_spriteBatch, a, 6, 6, 50f, 6);
    }

    public void Update(GameTime gameTime)
    {
        _animation.Update(gameTime);
    }

    public void Draw(Vector2 pos)
    {
        _animation.Draw(pos);
    }
}