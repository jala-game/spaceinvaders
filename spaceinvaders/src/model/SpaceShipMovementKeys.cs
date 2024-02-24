using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model.barricades;
using spaceinvaders.screen_logic.screens;

namespace spaceinvaders.model;

public static class SpaceShipMovementKeys
{
    public static Keys Left { get; private set; } = Keys.Left;
    public static Keys Right { get; private set; } = Keys.Right;
    public static Keys Shoot { get; private set; } = Keys.Space;
    public static Keys CurrentKeyToAssign { get; private set; }
    public static bool IsWaitingForKeyPress { get; private set; }

    public static void HandleEnterKeyPress(EControlOptions selectedOption, Game game)
    {
        switch (selectedOption)
        {
            case EControlOptions.Left:
                IsWaitingForKeyPress = true;
                CurrentKeyToAssign = Keys.Left;
                break;
            case EControlOptions.Right:
                IsWaitingForKeyPress = true;
                CurrentKeyToAssign = Keys.Right;
                break;
            case EControlOptions.Shoot:
                IsWaitingForKeyPress = true;
                CurrentKeyToAssign = Keys.Space;
                break;
            case EControlOptions.Exit:
                MainScreen mainScreen = new MainScreen(game, game.Services.GetService<GraphicsDeviceManager>(),
                    game.Content, game.Services.GetService<SpriteBatch>());
                ScreenManager.ChangeScreen(mainScreen);
                break;
        }
    }

    public static void AssignKey(Keys currentKey, Keys newKey, GameControlScreen gameControlScreen)
    {
        switch (currentKey)
        {
            case Keys.Left:
                Left = newKey;
                break;
            case Keys.Right:
                Right = newKey;
                break;
            case Keys.Space:
                Shoot = newKey;
                break;
        }

        // Atualiza o texto na tela com a nova tecla atribuída
        EControlOptions option = currentKey switch
        {
            Keys.Left => EControlOptions.Left,
            Keys.Right => EControlOptions.Right,
            Keys.Space => EControlOptions.Shoot,
            _ => EControlOptions.None
        };

        if (option != EControlOptions.None)
        {
            gameControlScreen.UpdateKeyText(option, newKey);
        }

        ResetWaitingForKeyPress();
    }


    public static void ResetWaitingForKeyPress()
    {
        IsWaitingForKeyPress = false;
    }
}