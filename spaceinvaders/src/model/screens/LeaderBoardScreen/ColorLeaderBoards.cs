using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace spaceinvaders.model;

public class ColorLeaderBoards
{
    private Color[] colors;
    
    public ColorLeaderBoards(){ ModifyColors();}
    
    public Color RandomColor(int numberOfColor)
    {
        if (numberOfColor > colors.Length || numberOfColor == -1) return Color.White;
        return colors[numberOfColor];
    }

    public void ModifyColors()
    {
        if (colors != null)
        {
            colors.Shuffle(new Random());
            return;
        }
        
        colors = new[] { Color.White, Color.Red, Color.Yellow, Color.Orange, Color.Green, Color.Cyan, 
            Color.Blue,Color.Azure,Color.Gold, Color.Magenta};
        
    }
}