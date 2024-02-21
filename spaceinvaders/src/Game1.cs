using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.screen_logic.screens;

namespace spaceinvaders;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;

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
        _background = Content.Load<Texture2D>("background");
        Content.Load<Texture2D>("barricades/barricade");
        Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        Content.Load<SpriteFont>("fonts/PixeloidMono");
        Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    }

    private void LoadScreenManager() {
        SaveScoreScreen saveScoreScreen = new(this, _graphics, Content, _spriteBatch, 100);
        ScreenManager.ChangeScreen(saveScoreScreen);
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

        _spriteBatch.Draw(_background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        ScreenManager.Draw(gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
