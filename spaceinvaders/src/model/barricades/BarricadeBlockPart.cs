using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;

namespace spaceinvaders.model;

public class BarricadeBlockPart : DrawableGameComponent, ICollisionActor
{
    private Texture2D _texture2D;
    public Rectangle Rectangle { get; set; }
    private Point _point;
    private BarricadeGeometry _barricadeGeometry;
    public short Life { get; set; } = 4;

    public BarricadeBlockPart(Game game, BarricadeGeometry barricadeGeometry, Point point) : base(game)
    {
        _point = point;
        _barricadeGeometry = barricadeGeometry;
        Game.Components.Add(this);
        Initialize();
    }

    public override void Initialize()
    {
        _texture2D = CropTexture(BarricadeFormatList.GetFormat(_barricadeGeometry));
        Bounds = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
        base.Initialize();
    }

    private Texture2D CropTexture(BarricadePositions b)
    {
        var croppedTexture = new Texture2D(GraphicsDevice, b.BlockSize, b.BlockSize);
        Rectangle = new Rectangle(_point, new Point(b.BlockSize, b.BlockSize));
        var data = new Color[b.BlockSize * b.BlockSize];
        var rectangle = new Rectangle(b.X, b.Y, b.BlockSize, b.BlockSize);
        Game.Content.Load<Texture2D>("barricades/barricade").GetData(0, rectangle, data, 0, b.BlockSize * b.BlockSize);
        croppedTexture.SetData(data);
        return croppedTexture;
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
        spriteBatch.Begin();
        spriteBatch.Draw(_texture2D, Rectangle, Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
        Console.WriteLine("Updated");
        base.Update(gameTime);
    }

    private void TakeDamage()
    {
        Console.WriteLine(Life);
        Life -= 1;
        if (Life <= 0)
        {
            Game.Components.Remove(this);
        }
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        if (collisionInfo.Other is Bullet)
        {
            TakeDamage();
            Console.WriteLine("a");
        }
    }

    public IShapeF Bounds { get; private set; }
}