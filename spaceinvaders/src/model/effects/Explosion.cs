using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Explosion {
    private Animation _animation;
    private Vector2 _pos;

    public Explosion(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        Texture2D a = content.Load<Texture2D>("effects/explosion");
        _animation = new(spriteBatch, a, 6, 6, 5f);
        _pos = pos;
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