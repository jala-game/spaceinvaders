using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace spaceinvaders.model;

public class ColorLeaderBoards
{
    private Color[] _colors;

    public ColorLeaderBoards()
    {
        ModifyColors();
    }

    public Color RandomColor(int numberOfColor)
    {
        if (numberOfColor > _colors.Length || numberOfColor == -1) return Color.White;
        return _colors[numberOfColor];
    }

    private void ModifyColors()
    {
        if (_colors != null)
        {
            _colors.Shuffle(new Random());
            return;
        }

        _colors =
        [
            Color.White, Color.Red, Color.Yellow, Color.Orange, Color.Green, Color.Cyan,
            Color.Blue, Color.Azure, Color.Gold, Color.Magenta
        ];
    }
}