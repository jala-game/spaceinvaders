using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model.effects;

public class CirclePurple
{
    private Animation _animation;
    private Vector2 _pos;

    public CirclePurple(SpriteBatch spriteBatch, ContentManager content, Vector2 pos)
    {
        Texture2D circlePurple = content.Load<Texture2D>("effects/circle-purple");
        _animation = new(spriteBatch, circlePurple, 5,5 , 15f, 1);
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