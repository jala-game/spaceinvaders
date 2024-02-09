using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class SpaceShip : Entity
{
    private readonly ContentManager _contentManager;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager graphics;

    private readonly int PLAYER_SPEED = 20;
    public readonly Texture2D texture;
    public Bullet bullet;

    public SpaceShip(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager)
    {
        Random random = new();
        var randomShip = random.Next(1, 5);
        texture = contentManager.Load<Texture2D>("ship2");


        var heightTop = _graphics.PreferredBackBufferHeight;
        var widthCenter = _graphics.PreferredBackBufferWidth / 2;

        var centralizeByTextureWidth = widthCenter - texture.Width / 2;
        var centralizeByTextureHeight = heightTop - texture.Height;

        var MARGIN = 50;
        var heightWithMargin = centralizeByTextureHeight - MARGIN;

        Vector2 position = new(centralizeByTextureWidth, heightWithMargin);

        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

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
            texture,
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
        var rightLimit = graphics.PreferredBackBufferWidth > Bounds.Position.X + texture.Width;
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
            bullet = new Bullet(Bounds.Position, bulletTexture, _spriteBatch, graphics, texture.Width);
        }
    }

    private void RemoveBulletWhenLeaveFromMap()
    {
        if (bullet != null && bullet.Bounds.Position.Y < 0) bullet = null;
    }
}