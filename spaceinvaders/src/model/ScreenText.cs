using Microsoft.Xna.Framework;

namespace spaceinvaders.model;

public class ScreenText(string text, Color textColor)
{
    public string Text { get; set; } = text;
    public Color TextColor { get; set; } = textColor;
}