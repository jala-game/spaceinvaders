using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model.effects;

public class Vortex
{
    private Animation _animation;
    private Vector2 _pos;

    public Vortex(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        Texture2D vortexTexture = content.Load<Texture2D>("effects/vortex");
        _animation = new(spriteBatch, vortexTexture, 5, 6, 5f);
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