using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model;
using spaceinvaders.model.barricades;

namespace spaceinvaders.screen_logic.screens;

public class GameControlScreen(Game game) : GameScreenModel
{
    private readonly SpriteFont _gameFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private readonly SpriteBatch _spriteBatch = game.Services.GetService<SpriteBatch>();
    private readonly GraphicsDeviceManager _deviceManager = game.Services.GetService<GraphicsDeviceManager>();
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
        var selectedOption = _selectedOption;

        switch (selectedOption)
        {
            case EControlOptions.Left:
                _waitingForKeyPress = true;
                _keyToAssign = Keys.Left;
                break;
            case EControlOptions.Right:
                _waitingForKeyPress = true;
                _keyToAssign = Keys.Right;
                break;
            case EControlOptions.Shoot:
                _waitingForKeyPress = true;
                _keyToAssign = Keys.Space; // Assuming space for shooting
                break;
            case EControlOptions.Exit:
                var mainScreen = new MainScreen(game, _deviceManager, game.Content, _spriteBatch);
                ScreenManager.ChangeScreen(mainScreen);
                break;
        }
    }

    private bool _waitingForKeyPress = false;
    private Keys _keyToAssign;

    private async Task<Keys> AwaitNextKeyPress()
    {
        await Task.Delay(10); // Pequeno atraso para evitar que a tecla Enter pressionada seja capturada imediatamente
        var kstate = Keyboard.GetState();
        if (!kstate.IsKeyDown(Keys.Enter))
        {
            Console.WriteLine(kstate.GetPressedKeys()[0]);
            return kstate.GetPressedKeys()[0];
        }

        return Keys.None;
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

        if (_waitingForKeyPress)
        {
            var pressedKeys = keyboardState.GetPressedKeys();
            if (pressedKeys.Length >  0)
            {
                AssignKey(_keyToAssign, pressedKeys[0]);
                _waitingForKeyPress = false;
            }
        }
        else
        {
            ModifyMenuSelection(keyboardState, gameTime);
        }

        base.Update(gameTime);
    }

    private void AssignKey(Keys currentKey, Keys newKey)
    {
        // Example logic to assign a new key to a control option
        // This should be adapted based on how you're managing control options
        switch (currentKey)
        {
            case Keys.Left:
                SpaceShipMovementKeys.Left = newKey;
                break;
            case Keys.Right:
                SpaceShipMovementKeys.Right = newKey;
                break;
            case Keys.Space:
                SpaceShipMovementKeys.Shoot = newKey;
                break;
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