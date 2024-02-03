using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class SpaceShip {
    private Texture2D texture;
    private Vector2 position;
    private GraphicsDeviceManager graphics;

    public SpaceShip(Texture2D playerTexture, Vector2 playerPosition, GraphicsDeviceManager _graphics) {
        texture = playerTexture;
        position = playerPosition;
        graphics = _graphics;
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

    public void Draw(SpriteBatch _spriteBatch)
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