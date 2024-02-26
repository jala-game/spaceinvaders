using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Explosion
{
    private readonly Animation _animation;
    private readonly Vector2 _pos;

    public Explosion(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        var a = content.Load<Texture2D>("effects/explosion");
        _animation = new Animation(spriteBatch, a, 6, 6, 5f, 6);
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