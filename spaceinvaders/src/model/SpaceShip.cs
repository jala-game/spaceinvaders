using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class SpaceShip {
    private readonly Texture2D texture;
    private Vector2 position;
    private GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;

    public SpaceShip(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, ContentManager contentManager) {
        texture = contentManager.Load<Texture2D>("ship");;
        position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);

        graphics = _graphics;
        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        var kstate = Keyboard.GetState();
        int PLAYER_SPEED = 20;

        bool rightLimit = graphics.PreferredBackBufferWidth > position.X + texture.Width / 2;
        if (kstate.IsKeyDown(Keys.D) && rightLimit) position.X += PLAYER_SPEED;

        bool leftLimit = texture.Width / 2 < position.X;
        if (kstate.IsKeyDown(Keys.A) && leftLimit) position.X -= PLAYER_SPEED;
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            texture,
            position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2, - graphics.PreferredBackBufferHeight / 2 + texture.Height + 50),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }
}