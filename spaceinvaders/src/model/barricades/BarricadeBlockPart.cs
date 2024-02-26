using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.services;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;

namespace spaceinvaders.model.barricades;

public class BarricadeBlockPart : DrawableGameComponent, IEntity
{
    public BarricadeBlockPart(Game game, BarricadeGeometry kindOfBarricadeGeometry, Point newPoint) : base(game)
    {
        NewPoint = newPoint;
        KindOfBarricadeGeometry = kindOfBarricadeGeometry;
        game.Components.Add(this);
        Initialize();
    }

    private Texture2D ContentBarricadeTexture2D { get; set; }
    private Rectangle PartRectangle { get; set; }
    private BarricadePositions PositionOfTheBarricadeIntoTheContentDraw { get; set; }
    private BarricadeGeometry KindOfBarricadeGeometry { get; }
    private Point NewPoint { get; }
    private short Life { get; set; }
    private List<IObserver> Observers { get; } = [];
    public IShapeF Bounds { get; private set; }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        TakeDamage();
    }


    public void Update()
    {
    }

    public void Draw()
    {
    }

    public override void Initialize()
    {
        ContentBarricadeTexture2D = CropTexture(BarricadeFormatList.GetFormat(KindOfBarricadeGeometry));
        Bounds = new RectangleF(PartRectangle.X, PartRectangle.Y, PartRectangle.Width, PartRectangle.Height);
        base.Initialize();
    }

    private Texture2D CropTexture(BarricadePositions b)
    {
        var croppedTexture = new Texture2D(GraphicsDevice, b.BlockSize, b.BlockSize);
        PartRectangle = new Rectangle(NewPoint, new Point(b.BlockSize));
        var data = new Color[b.BlockSize * b.BlockSize];
        var rectangle = new Rectangle(b.X, b.Y, b.BlockSize, b.BlockSize);
        Game.Content.Load<Texture2D>("barricades/barricade").GetData(0, rectangle, data, 0, b.BlockSize * b.BlockSize);
        croppedTexture.SetData(data);
        return croppedTexture;
    }

    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = Game.Services.GetService<SpriteBatch>();
        spriteBatch.Begin();
        spriteBatch.Draw(ContentBarricadeTexture2D, PartRectangle, Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }

    public void Attach(IObserver observer)
    {
        Observers.Add(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in Observers) observer.Notify(this);
    }

    private void TakeDamage()
    {
        Life += 1;
        var newSize = BarricadeFormatList.GetFormat(KindOfBarricadeGeometry).BlockSize;
        var newY = BarricadeFormatList.GetFormat(KindOfBarricadeGeometry).Y;
        var newX = BarricadeFormatList.GetFormat(KindOfBarricadeGeometry).X + newSize * Life;
        PositionOfTheBarricadeIntoTheContentDraw = new BarricadePositions(newX, newY, newSize);
        ContentBarricadeTexture2D = CropTexture(PositionOfTheBarricadeIntoTheContentDraw);
        if (Life < 3) return;
        NotifyObservers();
        Dispose();
    }

    protected override void Dispose(bool disposing)
    {
        Game.Components.Remove(this);
        Observers.Clear();
        base.Dispose(disposing);
    }
}