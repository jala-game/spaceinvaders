using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;

namespace spaceinvaders.model.barricades;

public class BarricadeBlockPart : DrawableGameComponent, Entity
{
    private Texture2D _texture2D;
    private Rectangle Rectangle { get; set; }
    private Point _point;
    private BarricadeGeometry _barricadeGeometry;
    private short Life { get; set; } = 4;

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

    private void TakeDamage()
    {
        Life -= 1;
        if (Life <= 0)
        {
            Game.Components.Remove(this);
        }
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        TakeDamage();
    }

    public IShapeF Bounds { get; private set; }

    public void Update()
    {
    }

    public void Draw()
    {
    }
}