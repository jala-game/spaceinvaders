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
        
        return colors[numberOfColor];
    }

    public void ModifyColors()
    {
        if (colors == null)
        {
            colors = new[] { Color.White, Color.Red, Color.Yellow, Color.Orange, Color.Green, Color.Cyan, 
                Color.Blue,Color.Azure,Color.Gold, Color.Magenta};
        }
        colors.Shuffle(new Random());
    }
}