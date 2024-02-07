using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class Bullet : Entity
{
    private readonly Texture2D _texture;
    private readonly float SPEED = 30f;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphics;

    public IShapeF Bounds { get; }

    public Bullet(Vector2 position, Texture2D texture, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        _texture = texture;
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));
        _spriteBatch = spriteBatch;
        _graphics = graphics;
    }

    public void Update()
    {
        Bounds.Position = new Vector2(Bounds.Position.X, Bounds.Position.Y - SPEED);
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            _texture,
            Bounds.Position,
            null,
            Color.White,
            0f,
            new Vector2(_texture.Width / 2, - _graphics.PreferredBackBufferHeight / 2 + _texture.Height + 50),
            Vector2.One,
            SpriteEffects.None,
            0f
        );

    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {

        Console.WriteLine("colidiu garaio");
    }
}
