using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model;

public interface Sprite
{
    protected Texture2D Texture { get; set; }
    protected Rectangle Rect { get; set; }
}