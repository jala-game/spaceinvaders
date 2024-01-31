using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace spaceinvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D playerTexture;
    Vector2 playerPosition;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height -100;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("ship");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var kstate = Keyboard.GetState();
        int PLAYER_SPEED = 20;

        bool rightLimit = _graphics.PreferredBackBufferWidth > playerPosition.X + playerTexture.Width / 2;
        if (kstate.IsKeyDown(Keys.D) && rightLimit) playerPosition.X += PLAYER_SPEED;

        bool leftLimit = playerTexture.Width / 2 < playerPosition.X;
        if (kstate.IsKeyDown(Keys.A) && leftLimit) playerPosition.X -= PLAYER_SPEED;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(
            playerTexture,
            playerPosition,
            null,
            Color.White,
            0f,
            new Vector2(playerTexture.Width / 2, - _graphics.PreferredBackBufferHeight / 2 + playerTexture.Height + 50),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
        // TODO: Add you+r drawing code here

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
