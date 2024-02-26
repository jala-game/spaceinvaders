using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model.effects;

public class StairExplosion
{
    private Animation _animation;
    private Vector2 _pos;

    public StairExplosion(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        Texture2D stairExplosion = content.Load<Texture2D>("effects/stair-explosion");
        _animation = new(spriteBatch, stairExplosion, 5, 4, 20f);
        _animation.repeat = true;
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