using System;
using System.Collections.Generic;
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

    private const float MenuMoveDelay = 0.15f;

    private float _menuMoveTimer;

    private readonly List<ScreenText> _listStrings =
    [
        new ScreenText("Left: " + SpaceShipMovementKeys.Left, Color.White),
        new ScreenText("Right: " + SpaceShipMovementKeys.Right, Color.White),
        new ScreenText("Shoot: " + SpaceShipMovementKeys.Shoot, Color.White),
        new ScreenText("Exit", Color.White)
    ];

    public GameControlScreen(Game game)
    {
        _spriteBatch = game.Services.GetService<SpriteBatch>();
        _deviceManager = game.Services.GetService<GraphicsDeviceManager>();
        _gameFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    }

    private void ModifyMenuSelection(KeyboardState kstate, GameTime gameTime)
    {
        _menuMoveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!(_menuMoveTimer >= MenuMoveDelay)) return;

        if (kstate.IsKeyDown(Keys.Enter))
        {
            HandleEnterKeyPress();
        }
        else if (kstate.IsKeyDown(Keys.Down) && (int)_selectedOption < 3 && _menuMoveTimer >= MenuMoveDelay)
        {
            _selectedOption++;
            _menuMoveTimer = 0f;
        }
        else if (kstate.IsKeyDown(Keys.Up) && _selectedOption > 0 && _menuMoveTimer >= MenuMoveDelay)
        {
            _selectedOption--;
            _menuMoveTimer = 0f;
        }

        ModifyScreenText();
    }

    private void HandleEnterKeyPress()
    {
        // Determine the currently selected option
        var selectedOption = _selectedOption;

        // Depending on the selected option, prompt for the new key and save it
        switch (selectedOption)
        {
            case EControlOptions.Left:
                SpaceShipMovementKeys.Left = AwaitNextKeyPress();
                break;
            case EControlOptions.Right:
                SpaceShipMovementKeys.Right = AwaitNextKeyPress();
                break;
            case EControlOptions.Shoot:
                SpaceShipMovementKeys.Shoot = AwaitNextKeyPress();
                break;
            case EControlOptions.Exit:

                break;
            default:
                // Handle invalid selection
                break;
        }
    }

    private Keys AwaitNextKeyPress()
    {
        while (true)
        {
            var kstate = Keyboard.GetState();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (kstate.IsKeyDown(key))
                {
                    return key;
                }
            }
        }
    }

    private void ModifyScreenText()
    {
        foreach (var text in _listStrings)
        {
            if (text.Text.StartsWith("> "))
            {
                text.Text = text.Text.Substring(2);
            }

            text.TextColor = Color.White;
        }

        var screenTextToBeModified = _listStrings[(int)_selectedOption];
        screenTextToBeModified.Text = "> " + screenTextToBeModified.Text;
        screenTextToBeModified.TextColor = Color.Green;
    }

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        ModifyMenuSelection(keyboardState, gameTime);
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