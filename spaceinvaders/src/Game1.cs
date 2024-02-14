using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using spaceinvaders.screen_logic.screens;

namespace spaceinvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D background;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 300;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height -100;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(typeof(SpriteBatch), _spriteBatch);
        Services.AddService(typeof(GraphicsDeviceManager), _graphics);
        LoadScreenManager();
        ScreenManager.Initialize();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        background = Content.Load<Texture2D>("background");
        Content.Load<Texture2D>("barricades/barricade");
    }

    private void LoadScreenManager() {
        SpaceShip spaceShip = new(_graphics, _spriteBatch, Content);
        PlayScreen playScreen = new(this, spaceShip, _graphics, Content, _spriteBatch);
        ScreenManager.ChangeScreen(playScreen);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ScreenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        ScreenManager.Draw(gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
