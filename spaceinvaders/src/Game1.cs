using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model;

namespace spaceinvaders;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private Barricades _barricades;

    private Texture2D _barricadeTexture;
    private SpriteBatch _spriteBatch;
    private Texture2D background;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 300;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
        _graphics.ApplyChanges();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(typeof(SpriteBatch), _spriteBatch);
        LoadScreenManager();
        ScreenManager.Initialize();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        background = Content.Load<Texture2D>("background");
        _barricadeTexture = Content.Load<Texture2D>("barricades/barricade");
        _barricades = new Barricades(this);
        Components.Add(_barricades);
    }

    private void LoadScreenManager()
    {
        SpaceShip spaceShip = new(_graphics, _spriteBatch, Content);
        PlayScreen playScreen = new(spaceShip, _graphics, Content, _spriteBatch);
        ScreenManager.ChangeScreen(playScreen);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ScreenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(background,
            new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

        ScreenManager.Draw(gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}