using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace spaceinvaders.model;

public class BarricadeBlockPart : DrawableGameComponent
{
    private Texture2D _texture2D;
    private Rectangle _rectangle;
    private Point _point;
    private BarricadeGeometry _barricadeGeometry;

    public BarricadeBlockPart(Game game, BarricadeGeometry barricadeGeometry, Point point) : base(game)
    {
        _point = point;
        _texture2D = CropTexture(BarricadeFormatList.GetFormat(_barricadeGeometry));
        _barricadeGeometry = barricadeGeometry;
    }

    private Texture2D CropTexture(BarricadePositions b)
    {
        var croppedTexture = new Texture2D(GraphicsDevice, b.BlockSize, b.BlockSize);
        _rectangle = new Rectangle(_point, new Point(b.BlockSize, b.BlockSize));
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
        spriteBatch.Draw(_texture2D, _rectangle, Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }
}