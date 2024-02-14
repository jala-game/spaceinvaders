using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;

public class SpaceShip : GameComponent, Entity
{
    private readonly ContentManager _contentManager;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager graphics;

    private readonly int PLAYER_SPEED = 20;
    public Texture2D Texture { get; set; }
    public Rectangle Rect { get; set; }
    public Bullet bullet;

    public SpaceShip(Game game, GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager) : base(game)
    {
        Random random = new();
        var randomShip = random.Next(1, 5);
        Texture = contentManager.Load<Texture2D>("ship2");


        var heightTop = _graphics.PreferredBackBufferHeight;
        var widthCenter = _graphics.PreferredBackBufferWidth / 2;

        var centralizeByTextureWidth = widthCenter - Texture.Width / 2;
        var centralizeByTextureHeight = heightTop - Texture.Height;

        var MARGIN = 50;
        var heightWithMargin = centralizeByTextureHeight - MARGIN;

        Vector2 position = new(centralizeByTextureWidth, heightWithMargin);

        Bounds = new RectangleF(position, new Size2(Texture.Width, Texture.Height));

        graphics = _graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        var kstate = Keyboard.GetState();

        MoveToRight(kstate);
        MoveToLeft(kstate);

        Shoot(kstate);
        RemoveBulletWhenLeaveFromMap();
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            Texture,
            Bounds.Position,
            Color.White
        );
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        bullet = null;
    }

    private void MoveToRight(KeyboardState kstate)
    {
        var rightLimit = graphics.PreferredBackBufferWidth > Bounds.Position.X + Texture.Width;
        if (kstate.IsKeyDown(Keys.D) && rightLimit)
        {
            Vector2 newPosition = new(PLAYER_SPEED + Bounds.Position.X, Bounds.Position.Y);
            Bounds.Position = newPosition;
        }
    }

    private void MoveToLeft(KeyboardState kstate)
    {
        var leftLimit = 0 < Bounds.Position.X;
        if (kstate.IsKeyDown(Keys.A) && leftLimit)
        {
            Vector2 newPosition = new(Bounds.Position.X - PLAYER_SPEED, Bounds.Position.Y);
            Bounds.Position = newPosition;
        }
    }

    private void Shoot(KeyboardState kstate)
    {
        if (kstate.IsKeyDown(Keys.Space) && bullet == null)
        {
            var bulletTexture = _contentManager.Load<Texture2D>("red-bullet");
            bullet = new Bullet(Game, Bounds.Position, bulletTexture, _spriteBatch, graphics, Texture.Width);
        }
    }

    private void RemoveBulletWhenLeaveFromMap()
    {
        if (bullet != null && bullet.Bounds.Position.Y < 0) bullet = null;
    }
}