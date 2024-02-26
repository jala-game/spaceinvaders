using Microsoft.Xna.Framework.Input;

namespace spaceinvaders.model;

public static class SpaceShipMovementKeys
{
    public static Keys Left { get; private set; } = Keys.Left;
    public static Keys Right { get; private set; } = Keys.Right;
    public static Keys KeyA { get; private set; } = Keys.A;
    public static Keys KeyD { get; private set; } = Keys.D;
    public static Keys Shoot { get; private set; } = Keys.Space;
}