using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.enums;
using spaceinvaders.model.barricades;
using spaceinvaders.utils;

namespace spaceinvaders.model;

public class SpaceShip : IEntity
{
    private readonly ContentManager _contentManager;
    private readonly SpriteBatch _spriteBatch;
    private readonly Game _game;
    private readonly GraphicsDeviceManager _graphics;

    private const int PlayerSpeed = 10;
    private readonly Texture2D _texture;
    private bool _isDead;
    private int _numberOfLives;
    public Bullet Bullet;

    public SpaceShip(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager,
        Game game)
    {
        _texture = contentManager.Load<Texture2D>("ship1");


        var heightTop = graphics.PreferredBackBufferHeight;
        var widthCenter = graphics.PreferredBackBufferWidth / 2;

        var centralizeByTextureWidth = widthCenter - _texture.Width / 2;
        var centralizeByTextureHeight = heightTop - _texture.Height;

        const int margin = 50;
        var heightWithMargin = centralizeByTextureHeight - margin;

        Vector2 position = new(centralizeByTextureWidth, heightWithMargin);

        Bounds = new RectangleF(position, new Size2(_texture.Width, _texture.Height));

        this._graphics = graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
        _isDead = false;
        _numberOfLives = 3;

        this._game = game;
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        var kstate = Keyboard.GetState();

        MoveToRight(kstate);
        MoveToLeft(kstate);

        Shoot(kstate);
        RemoveBulletWhenLeaveFromMap();
        RemoveBulletIfIsDead();
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
        RemoveLifeForShip();
    }

    private void MoveToRight(KeyboardState kstate)
    {
        var rightLimit = _graphics.PreferredBackBufferWidth > Bounds.Position.X + _texture.Width;
        if ((!kstate.IsKeyDown(SpaceShipMovementKeys.Right) && !kstate.IsKeyDown(SpaceShipMovementKeys.KeyD)) ||
            !rightLimit) return;
        Vector2 newPosition = new(PlayerSpeed + Bounds.Position.X, Bounds.Position.Y);
        Bounds.Position = newPosition;
    }

    private void MoveToLeft(KeyboardState kstate)
    {
        var leftLimit = 0 < Bounds.Position.X;
        if ((kstate.IsKeyDown(SpaceShipMovementKeys.Left) || kstate.IsKeyDown(SpaceShipMovementKeys.KeyA)) && leftLimit)
        {
            Vector2 newPosition = new(Bounds.Position.X - PlayerSpeed, Bounds.Position.Y);
            Bounds.Position = newPosition;
        }

        ;
    }

    private void Shoot(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(SpaceShipMovementKeys.Shoot) || Bullet != null) return;
        var bulletTexture = _contentManager.Load<Texture2D>("blue-bullet");
        SoundEffects.LoadEffect(_game, ESoundsEffects.ShootSpaceShip);
        SoundEffects.PlaySoundEffect();
        Bullet = new Bullet(Bounds.Position, bulletTexture, _spriteBatch, _graphics, _texture.Width,
            TypeBulletEnum.Player);
    }

    private void RemoveBulletWhenLeaveFromMap()
    {
        if (Bullet != null && Bullet.Bounds.Position.Y < 0) Bullet = null;
    }

    private void RemoveBulletIfIsDead()
    {
        if (Bullet != null && Bullet.GetIsDead()) Bullet = null;
    }

    public void AddLifeForShip()
    {
        if (_numberOfLives < 6) _numberOfLives += 1;
    }

    private void RemoveLifeForShip()
    {
        SoundEffects.LoadEffect(_game, ESoundsEffects.SpaceShipDead);
        SoundEffects.PlaySoundEffect(0.2f);
        if (_numberOfLives > 1)
        {
            _numberOfLives -= 1;
            return;
        }

        _isDead = true;
    }

    public bool GetIsDead()
    {
        return _isDead;
    }

    public void SetIsDead()
    {
        _isDead = true;
    }

    public int GetLives()
    {
        return _numberOfLives;
    }
}