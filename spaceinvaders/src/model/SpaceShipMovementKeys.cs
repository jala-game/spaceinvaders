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
    public static Keys KeyA { get; private set; } = Keys.A;
    public static Keys KeyD { get; private set; } = Keys.D;
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
            case EControlOptions.KeyA:
                IsWaitingForKeyPress = true;
                CurrentKeyToAssign = Keys.A;
                break;
            case EControlOptions.KeyD:
                IsWaitingForKeyPress = true;
                CurrentKeyToAssign = Keys.D;
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
            case Keys.A:
                Left = newKey;
                break;
            case Keys.D:
                Right = newKey;
                break;
            case Keys.Space:
                Shoot = newKey;
                break;
        }

        EControlOptions option = currentKey switch
        {
            Keys.Left => EControlOptions.Left,
            Keys.Right => EControlOptions.Right,
            Keys.A => EControlOptions.KeyA,
            Keys.V => EControlOptions.KeyD,
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