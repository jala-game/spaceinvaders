using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class Bullet : Entity
{
    private readonly Texture2D _texture;
    private readonly float SPEED = 25f;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphics;

    public IShapeF Bounds { get; }

    public Bullet(Vector2 position, Texture2D texture, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int shipTextureWidth)
    {
        _texture = texture;
        float bulletWidth = texture.Width / 2;
        float shipWidth = shipTextureWidth / 2;
        Vector2 bulletPosition = new(position.X + shipWidth - bulletWidth, position.Y + texture.Height / 2 + 50);
        Bounds = new RectangleF(bulletPosition, new Size2(texture.Width, texture.Height));
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
            Color.White
        );

    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {

        // 
    }
}
