using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;

public class Bullet : DrawableGameComponent, Entity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private readonly float SPEED = 25f;

    public Bullet(Game game, Vector2 position, Texture2D texture, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,
        int shipTextureWidth) : base(game)
    {
        _texture = texture;
        float bulletWidth = texture.Width / 2;
        float shipWidth = shipTextureWidth / 2;
        Vector2 bulletPosition = new(position.X + shipWidth - bulletWidth, position.Y + texture.Height / 2 + 50);
        Bounds = new RectangleF(bulletPosition, new Size2(texture.Width, texture.Height));
        _spriteBatch = spriteBatch;
        _graphics = graphics;
        Game.Components.Add(this);
    }

    public IShapeF Bounds { get; }

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
        if (collisionInfo.Other is BarricadeBlockPart)
        {
            ((BarricadeBlockPart)collisionInfo.Other).TakeDamage();
        }
    }
}