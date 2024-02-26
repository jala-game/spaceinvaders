using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.enums;

namespace spaceinvaders.model.aliens.ships.queue_aliens;

public class ShooterEnemy : IEnemyGroup
{
    private readonly ContentManager _contentManager;

    private readonly GraphicsDeviceManager _graphics;
    private readonly int _point = 40;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private Bullet _bullet;
    private bool _directionRight = true;
    private bool _isDead;

    public ShooterEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x,
        int y)
    {
        var texture = contentManager.Load<Texture2D>("aliens/shooter-alien-ship");
        _graphics = graphics;
        _texture = texture;

        var height = y;
        var width = x - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
    }

    public IShapeF Bounds { get; }

    public bool IsDead()
    {
        return _isDead;
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        _isDead = true;
    }

    public void Update()
    {
        var randomShotValue = RandomShotValue();
        Shoot(randomShotValue);
        RemoveBulletIfIsDead();
        RemoveBulletWhenLeaveFromMap();
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            _texture,
            Bounds.Position,
            Color.White
        );
    }

    public void InvertDirection()
    {
        _directionRight = !_directionRight;
    }

    public void IncreaseX(float value)
    {
        var valueModified = _directionRight ? value : -value;
        Bounds.Position += new Vector2(valueModified, 0);
    }

    public void Fall()
    {
        Bounds.Position += new Vector2(0, 50);
    }

    public Bullet GetBullet()
    {
        return _bullet;
    }

    public Texture2D GetTexture()
    {
        return _texture;
    }

    public int GetPoint()
    {
        return _point;
    }

    private void Shoot(int randomShotValue)
    {
        if (randomShotValue != 5 || _bullet != null) return;
        var bulletTexture = _contentManager.Load<Texture2D>("red-bullet");
        _bullet = new Bullet(Bounds.Position, bulletTexture, _spriteBatch, _graphics, _texture.Width,
            TypeBulletEnum.Alien);
    }

    private void RemoveBulletWhenLeaveFromMap()
    {
        if (_bullet != null && _bullet.Bounds.Position.Y > _graphics.PreferredBackBufferHeight) _bullet = null;
    }

    private void RemoveBulletIfIsDead()
    {
        if (_bullet != null && _bullet.GetIsDead()) _bullet = null;
    }

    private static int RandomShotValue()
    {
        var random = new Random();
        return random.Next(0, 500);
    }
}