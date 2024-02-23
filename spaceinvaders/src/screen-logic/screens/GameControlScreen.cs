using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model.barricades;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen : GameScreenModel
{
    private readonly SpriteFont _gameFont;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _deviceManager;
    private EControlOptions _selectedOption = EControlOptions.Left;
    private readonly List<string> _listStrings = ["Left", "Right", "Shoot", "Exit"];

    public GameControlScreen(Game game)
    {
        _spriteBatch = game.Services.GetService<SpriteBatch>();
        _deviceManager = game.Services.GetService<GraphicsDeviceManager>();
        _gameFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Up) && _selectedOption > 0)
        {
            _selectedOption--;
        }
        else if (kstate.IsKeyDown(Keys.Down) && (int)_selectedOption < 2)
        {
            _selectedOption++;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        var height = _deviceManager.PreferredBackBufferHeight / 2;
        var width = _deviceManager.PreferredBackBufferWidth / 2;
        var middleOfScreen = new Rectangle(width, height, 0, 0);
        DrawNormalText(_listStrings, middleOfScreen, 100);
        base.Draw(gameTime);
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

    private void DrawNormalText(List<string> texts, Rectangle textPosition, int gapY, Color color)
    {
        var totalTextHeight = texts.Count * (_gameFont.LineSpacing + gapY) - gapY;
        var startY = textPosition.Y + (textPosition.Height - totalTextHeight) / 2;

        foreach (var text in texts)
        {
            var textWidth = _gameFont.MeasureString(text).X;
            var startX = textPosition.X + (textPosition.Width - textWidth) / 2;
            var newRectangle = new Rectangle((int)startX, startY, (int)textWidth, totalTextHeight);
            DrawNormalText(text, newRectangle, color);
            startY += _gameFont.LineSpacing + gapY;
        }
    }

    private void DrawNormalText(List<string> texts, Rectangle textPosition, int gapY)
    {
        DrawNormalText(texts, textPosition, gapY, Color.White);
    }
}