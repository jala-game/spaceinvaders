using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.enums;

namespace spaceinvaders.model;

public class Bullet : IEntity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;

    private readonly TypeBulletEnum _typeBulletEnum;
    private const float Speed = 14f;
    private bool _isDead;

    public Bullet(Vector2 position, Texture2D texture, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,
        int shipTextureWidth, TypeBulletEnum typeBulletEnum)
    {
        _texture = texture;
        float bulletWidth = texture.Width / 2;
        float shipWidth = shipTextureWidth / 2;
        var bulletSpacement = typeBulletEnum == TypeBulletEnum.Player ? 50 : 0;
        Vector2 bulletPosition = new(position.X + shipWidth - bulletWidth,
            position.Y + texture.Height / 2 + bulletSpacement);
        Bounds = new RectangleF(bulletPosition, new Size2(texture.Width, texture.Height));
        _spriteBatch = spriteBatch;
        _graphics = graphics;
        _typeBulletEnum = typeBulletEnum;
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        Bounds.Position = _typeBulletEnum switch
        {
            TypeBulletEnum.Player => new Vector2(Bounds.Position.X, Bounds.Position.Y - Speed),
            TypeBulletEnum.Alien => new Vector2(Bounds.Position.X, Bounds.Position.Y + Speed),
            _ => Bounds.Position
        };
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
        _isDead = true;
    }

    public bool GetIsDead()
    {
        return _isDead;
    }
}