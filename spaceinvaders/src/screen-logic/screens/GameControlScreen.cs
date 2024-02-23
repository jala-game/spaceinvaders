using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen : GameScreenModel, IGameComponent
{
    private readonly Game Game;
    private readonly SpriteFont _gameFont;
    private readonly SpriteFont _gameFontSmall;
    private readonly SpriteFont _gameDescription;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _deviceManager;

    public GameControlScreen(Game game)
    {
        Game = game;
        _spriteBatch = game.Services.GetService<SpriteBatch>();
        _deviceManager = Game.Services.GetService<GraphicsDeviceManager>();
        _gameFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        _gameFontSmall = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
        _gameDescription = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
    }

    public override void Initialize()
    {
        var _listStrings
        DrawNormalText();
        base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    private void DrawNormalText(string text, Rectangle textPosition, Color color)
    {
        _spriteBatch.DrawString(_gameFont, text,
            new Vector2(textPosition.X, textPosition.Y), color);
    }

    private void DrawNormalText(string text, Rectangle textPosition)
    {
        DrawNormalText(text, textPosition, Color.White);
    }

    private void DrawNormalText(List<string> texts, Rectangle textPosition, int gapX, int gapY, Color color)
    {
        var x = 0;
        var y = 0;
        foreach (var text in texts)
        {
            _spriteBatch.DrawString(_gameFont, text,
                new Vector2(textPosition.X + x, textPosition.Y + y), color);
            x += gapX;
            y += gapY;
        }
    }

    private void DrawNormalText(List<string> texts, Rectangle textPosition, int gapX, int gapY)
    {
        DrawNormalText(texts, textPosition, gapX, gapY, Color.White);
    }
}