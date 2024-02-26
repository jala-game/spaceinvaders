using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model.barricades;
using spaceinvaders.utils;

namespace spaceinvaders.model.aliens.ships;

public class RedEnemy : IEnemyEntity
{
    private const float AlienSpeedX = 2.5f;
    private readonly GraphicsDeviceManager _graphics;
    private readonly int _isRightOrLeft;
    private readonly Random _random = new();
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool _isDead;
    private float _rotator;


    public RedEnemy(Game game, ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        var texture = contentManager.Load<Texture2D>("aliens/red-alien-ship");
        _graphics = graphics;

        _isRightOrLeft = _random.Next(0, 2);

        const int height = 30;
        var widthInitialLocation = _isRightOrLeft == 0 ? -50 : _graphics.PreferredBackBufferWidth + 50;
        var width = widthInitialLocation - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
        _texture = texture;
        SoundEffects.LoadEffect(game, ESoundsEffects.RedShip);
        SoundEffects.PlaySoundEffect(0.7f);
    }

    public IShapeF Bounds { get; }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        _isDead = true;
    }

    public void Update()
    {
        _rotator += 0.05f;
        IsOutsideFromMap();
        Movement();
    }

    public void Draw()
    {
        Vector2 origin = new(_texture.Width / 2, _texture.Height / 2);
        _spriteBatch.Draw(
            _texture,
            new Vector2(Bounds.Position.X + _texture.Width / 2, Bounds.Position.Y + _texture.Height / 2),
            null,
            Color.White,
            _rotator,
            origin,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public bool IsDead()
    {
        return _isDead;
    }

    private void IsOutsideFromMap()
    {
        var rightLimit = _graphics.PreferredBackBufferWidth > Bounds.Position.X + _texture.Width / 2;
        var leftLimit = 0 - _texture.Width < Bounds.Position.X;
        if ((_isRightOrLeft == 0 && !rightLimit) || (_isRightOrLeft == 1 && !leftLimit)) _isDead = true;
    }

    private void Movement()
    {
        switch (_isRightOrLeft)
        {
            case 0:
                Vector2 onceRight = new(AlienSpeedX, 0);
                Bounds.Position += onceRight;
                break;
            case 1:
                Vector2 onceLeft = new(-AlienSpeedX, 0);
                Bounds.Position += onceLeft;
                break;
        }
    }

    public static int GetPoint()
    {
        Random random = new();
        int[] points = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100];
        return points[random.Next(0, points.Length)];
    }
}