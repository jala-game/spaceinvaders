using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
    private const float MenuMoveDelay =  0.15f;
    private float _menuMoveTimer;

    private readonly List<ScreenText> _listStrings =
    [
        new ScreenText("Left: ", Color.White),
        new ScreenText("Right: ", Color.White),
        new ScreenText("Shoot: ", Color.White),
        new ScreenText("Exit", Color.White)
    ];

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (SpaceShipMovementKeys.IsWaitingForKeyPress)
        {
            HandleKeyAssignment(keyboardState);
        }
        else
        {
            ModifyMenuSelection(keyboardState, gameTime);
        }

        base.Update(gameTime);
    }

    public void UpdateKeyText(EControlOptions option, Keys newKey)
    {
        var textToUpdate = _listStrings[(int)option];
        textToUpdate.Text = $"{option}: {newKey}";
    }

    private void ModifyMenuSelection(KeyboardState kstate, GameTime gameTime)
    {
        _menuMoveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!(_menuMoveTimer >= MenuMoveDelay)) return;

        if (kstate.IsKeyDown(Keys.Enter))
        {
            HandleEnterKeyPress();
        }
        else if (kstate.IsKeyDown(Keys.Down) && (int)_selectedOption <  3 && _menuMoveTimer >= MenuMoveDelay)
        {
            _selectedOption++;
            _menuMoveTimer =  0f;
        }
        else if (kstate.IsKeyDown(Keys.Up) && _selectedOption >  0 && _menuMoveTimer >= MenuMoveDelay)
        {
            _selectedOption--;
            _menuMoveTimer =  0f;
        }

        ModifyScreenText();
    }

    private void HandleEnterKeyPress()
    {
        SpaceShipMovementKeys.HandleEnterKeyPress(_selectedOption, game);
    }

    private void ModifyScreenText()
    {
        foreach (var text in _listStrings)
        {
            // Remover a indicação de tecla atribuída se já existir
            if (text.Text.StartsWith("> "))
            {
                text.Text = text.Text.Substring(2);
            }

            // Verificar se o texto já contém a indicação de tecla atribuída
            if (!text.Text.Contains(" - "))
            {
                // Adicionar a indicação de tecla atribuída apenas se não existir
                text.Text += " - " + GetKeyNameForOption((EControlOptions)(_listStrings.IndexOf(text)));
            }

            text.TextColor = Color.White;
        }

        var screenTextToBeModified = _listStrings[(int)_selectedOption];
        screenTextToBeModified.Text = "> " + screenTextToBeModified.Text;
        screenTextToBeModified.TextColor = Color.Green;
    }


    private string GetKeyNameForOption(EControlOptions option)
    {
        switch (option)
        {
            case EControlOptions.Left:
                return SpaceShipMovementKeys.Left.ToString();
            case EControlOptions.Right:
                return SpaceShipMovementKeys.Right.ToString();
            case EControlOptions.Shoot:
                return SpaceShipMovementKeys.Shoot.ToString();
            default:
                return string.Empty;
        }
    }

    private void HandleKeyAssignment(KeyboardState keyboardState)
    {
        var pressedKeys = keyboardState.GetPressedKeys();
        if (pressedKeys.Length <=  0) return;
        SpaceShipMovementKeys.AssignKey(SpaceShipMovementKeys.CurrentKeyToAssign, pressedKeys[0], this);
        SpaceShipMovementKeys.ResetWaitingForKeyPress();
    }


    public override void Draw(GameTime gameTime)
    {
        var height = _deviceManager.PreferredBackBufferHeight /  2;
        var width = _deviceManager.PreferredBackBufferWidth /  2;
        var middleOfScreen = new Rectangle(width, height,  0,  0);
        DrawNormalText(_listStrings, middleOfScreen,  100);
        base.Draw(gameTime);
    }

    private void DrawNormalText(List<ScreenText> texts, Rectangle textPosition, int gapY)
    {
        var totalTextHeight = texts.Count * (_gameFont.LineSpacing + gapY) - gapY;
        var startY = textPosition.Y + (textPosition.Height - totalTextHeight) /  2;

        foreach (var text in texts)
        {
            var textWidth = _gameFont.MeasureString(text.Text).X;
            var startX = textPosition.X + (textPosition.Width - textWidth) /  2;
            var newRectangle = new Rectangle((int)startX, startY, (int)textWidth, totalTextHeight);
            DrawNormalText(text, newRectangle);
            startY += _gameFont.LineSpacing + gapY;
        }
    }

    private void DrawNormalText(ScreenText text, Rectangle textPosition)
    {
        _spriteBatch.DrawString(_gameFont, text.Text,
            new Vector2(textPosition.X, textPosition.Y), text.TextColor);
    }
}