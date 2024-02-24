using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;

public class SpaceShip : Entity {
    public readonly Texture2D texture;
    private readonly GraphicsDeviceManager graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly ContentManager _contentManager;
    private readonly Keys _keysLeft;
    private readonly Keys _keysRight;
    private readonly Keys _keysShoot;

    public IShapeF Bounds { get; }
    public Bullet bullet;
    private bool _isDead;
    private int _numberOfLives;

    private readonly int PLAYER_SPEED = 10;

    public SpaceShip(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager) {
        Random random = new();
        // int randomShip = random.Next(1, 3);
        texture = contentManager.Load<Texture2D>($"ship1");


        int heightTop = _graphics.PreferredBackBufferHeight;
        int widthCenter = _graphics.PreferredBackBufferWidth / 2;

        int centralizeByTextureWidth = widthCenter - texture.Width / 2;
        int centralizeByTextureHeight = heightTop - texture.Height;

        int MARGIN = 50;
        int heightWithMargin = centralizeByTextureHeight - MARGIN;

        Vector2 position = new(centralizeByTextureWidth, heightWithMargin);

        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        graphics = _graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
        _isDead = false;
        _numberOfLives = 3;
    }

    public void Update()
    {
        var kstate = Keyboard.GetState();

        MoveToRight(kstate);
        MoveToLeft(kstate);

        Shoot(kstate);
        RemoveBulletWhenLeaveFromMap();
        RemoveBulletIfIsDead();
    }

    private void MoveToRight(KeyboardState kstate) {
        bool rightLimit = graphics.PreferredBackBufferWidth > Bounds.Position.X + texture.Width;
        if (kstate.IsKeyDown(SpaceShipMovementKeys.Right) && rightLimit) {
            Vector2 newPosition = new(PLAYER_SPEED + Bounds.Position.X, Bounds.Position.Y);
            Bounds.Position = newPosition;
        }
    }

    private void MoveToLeft(KeyboardState kstate) {
        bool leftLimit = 0 < Bounds.Position.X;
        if (kstate.IsKeyDown(SpaceShipMovementKeys.Left) && leftLimit) {
            Vector2 newPosition = new(Bounds.Position.X - PLAYER_SPEED, Bounds.Position.Y);
            Bounds.Position = newPosition;
        };
    }

    private void Shoot(KeyboardState kstate) {
        if (kstate.IsKeyDown(SpaceShipMovementKeys.Shoot) && bullet == null) {
            Texture2D bulletTexture = _contentManager.Load<Texture2D>("red-bullet");
            bullet = new Bullet(Bounds.Position, bulletTexture, _spriteBatch, graphics, texture.Width, TypeBulletEnum.PLAYER);
        }
    }

    private void RemoveBulletWhenLeaveFromMap() {
        if (bullet != null && bullet.Bounds.Position.Y < 0) {
            bullet = null;
        }
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            texture,
            Bounds.Position,
            Color.White
        );
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        RemoveLifeForShip();
    }

    public void RemoveBulletIfIsDead()
    {
        if (bullet != null && bullet.GetIsDead()) bullet = null;
    }

    public void AddLifeForShip()
    {
        if (_numberOfLives < 6) _numberOfLives += 1;
    }

    private void RemoveLifeForShip()
    {
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

    public int GetLifes()
    {
        return _numberOfLives;
    }
}