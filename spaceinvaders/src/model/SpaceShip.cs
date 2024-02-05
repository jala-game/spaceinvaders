using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class SpaceShip : Entity {
    private readonly Texture2D texture;
    private GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;

    public IShapeF Bounds { get; }

    public SpaceShip(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager) {
        Random random = new();
        int randomShip = random.Next(1, 4);
        texture = contentManager.Load<Texture2D>($"ship{randomShip}");
        Vector2 position = new(_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);
        Bounds = new RectangleF(position, new Size2(100, 90));

        graphics = _graphics;
        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        var kstate = Keyboard.GetState();
        int PLAYER_SPEED = 20;

        bool rightLimit = graphics.PreferredBackBufferWidth > Bounds.Position.X + texture.Width / 2;
        if (kstate.IsKeyDown(Keys.D) && rightLimit) Bounds.Position = new Vector2(PLAYER_SPEED + Bounds.Position.X, Bounds.Position.Y);

        bool leftLimit = texture.Width / 2 < Bounds.Position.X;
        if (kstate.IsKeyDown(Keys.A) && leftLimit) Bounds.Position = new Vector2(Bounds.Position.X - PLAYER_SPEED, Bounds.Position.Y);
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
        
    }
}