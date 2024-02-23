using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model;
using spaceinvaders.model.barricades;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen : GameScreenModel
{
    private readonly SpriteFont _gameFont;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _deviceManager;
    private EControlOptions _selectedOption = EControlOptions.Left;
    private readonly List<ScreenText> _listStrings =
    [
        new ScreenText("Left", Color.White),
        new ScreenText("Right", Color.White),
        new ScreenText("Shoot", Color.White),
        new ScreenText("Exit", Color.White)
    ];

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
            Thread.Sleep(500);
        }
        else if (kstate.IsKeyDown(Keys.Down) && (int)_selectedOption < 3)
        {
            _selectedOption++;
            Thread.Sleep(500);
        }
        ModifyScreenText();
    }

    private void ModifyScreenText()
    {
        foreach (var text in _listStrings)
        {
            text.TextColor = Color.White;
        }
        _listStrings[(int)_selectedOption].TextColor = Color.Green;
    }

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        ModifyMenuSelection(keyboardState);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        var height = _deviceManager.PreferredBackBufferHeight / 2;
        var width = _deviceManager.PreferredBackBufferWidth / 2;
        var middleOfScreen = new Rectangle(width, height, 0, 0);
        DrawNormalText(_listStrings, middleOfScreen, 100);
        base.Draw(gameTime);
    }

    private void DrawNormalText(ScreenText text, Rectangle textPosition)
    {
        _spriteBatch.DrawString(_gameFont, text.Text,
            new Vector2(textPosition.X, textPosition.Y), text.TextColor);
    }

    private void DrawNormalText(List<ScreenText> texts, Rectangle textPosition, int gapY)
    {
        var totalTextHeight = texts.Count * (_gameFont.LineSpacing + gapY) - gapY;
        var startY = textPosition.Y + (textPosition.Height - totalTextHeight) / 2;

        foreach (var text in texts)
        {
            var textWidth = _gameFont.MeasureString(text.Text).X;
            var startX = textPosition.X + (textPosition.Width - textWidth) / 2;
            var newRectangle = new Rectangle((int)startX, startY, (int)textWidth, totalTextHeight);
            DrawNormalText(text, newRectangle);
            startY += _gameFont.LineSpacing + gapY;
        }
    }
}