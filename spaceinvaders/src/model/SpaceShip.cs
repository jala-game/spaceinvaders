using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class SpaceShip : Entity {
    public readonly Texture2D texture;
    private GraphicsDeviceManager graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly ContentManager _contentManager;

    public IShapeF Bounds { get; }
    public Bullet bullet;

    private readonly int PLAYER_SPEED = 20;

    public SpaceShip(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager) {
        Random random = new();
        int randomShip = random.Next(1, 4);
        texture = contentManager.Load<Texture2D>($"ship{randomShip}");

        Vector2 position = new(_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        graphics = _graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        var kstate = Keyboard.GetState();

        MoveToRight(kstate);
        MoveToLeft(kstate);

        Shoot(kstate);
        RemoveBulletWhenLeaveFromMap();
    }

    private void MoveToRight(KeyboardState kstate) {
        bool rightLimit = graphics.PreferredBackBufferWidth > Bounds.Position.X + texture.Width / 2;
        if (kstate.IsKeyDown(Keys.D) && rightLimit) {
            Vector2 newPosition = new(PLAYER_SPEED + Bounds.Position.X, Bounds.Position.Y);
            Bounds.Position = newPosition;
        }
    }

    private void MoveToLeft(KeyboardState kstate) {
        bool leftLimit = texture.Width / 2 < Bounds.Position.X;
        if (kstate.IsKeyDown(Keys.A) && leftLimit) {
            Vector2 newPosition = new(Bounds.Position.X - PLAYER_SPEED, Bounds.Position.Y);
            Bounds.Position = newPosition;
        };
    }

    private void Shoot(KeyboardState kstate) {
        if (kstate.IsKeyDown(Keys.Space) && bullet == null) {
            Texture2D bulletTexture = _contentManager.Load<Texture2D>("red-bullet");
            bullet = new Bullet(new Vector2(Bounds.Position.X, Bounds.Position.Y), bulletTexture, _spriteBatch, graphics);
        }
    }

    private void RemoveBulletWhenLeaveFromMap() {
        if (bullet != null && bullet.Bounds.Position.Y < -graphics.PreferredBackBufferHeight / 2) {
            bullet = null;
        }
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            texture,
            Bounds.Position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2, - graphics.PreferredBackBufferHeight / 2 + texture.Height + 50),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        bullet = null;
    }
}