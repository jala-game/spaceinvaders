using System.Collections.Generic;
using Microsoft.Xna.Framework;
using spaceinvaders.services;

namespace spaceinvaders.model.barricades;

public class BarricadeBlock : GameComponent, IObserver
{
    private const int BlockPartRows = 3;
    private const int BlockPartColumns = 4;
    public List<BarricadeBlockPart> BarricadeBlockParts { get; }
    private readonly Point _point;

    public BarricadeBlock(Game game, Point point) : base(game)
    {
        _point = point;
        BarricadeBlockParts = [];
        Initialize();
    }

    public override void Initialize()
    {
        var pseudoPoint = _point;
        var blockGap = BarricadeFormatList.GetFormat(BarricadeGeometry.Square).BlockSize;
        const int lastColumnIndex = BlockPartColumns - 1;
        const int lastRowIndex = BlockPartRows - 1;

        for (var i = 0; i < BlockPartRows; i++)
        {
            for (var j = 0; j < BlockPartColumns; j++)
            {
                BarricadeGeometry geometry;

                switch (j)
                {
                    case 0 when i is 0 or lastColumnIndex:
                        geometry = BarricadeGeometry.LeftTriangle;
                        break;
                    case lastColumnIndex when i is 0 or lastColumnIndex:
                        geometry = BarricadeGeometry.RightTriangle;
                        break;
                    default:
                    {
                        switch (i)
                        {
                            case lastRowIndex when j == 1:
                                geometry = BarricadeGeometry.LittleLeftTriangle;
                                break;
                            case lastRowIndex when j == lastColumnIndex - 1:
                                geometry = BarricadeGeometry.LittleRightTriangle;
                                pseudoPoint.X += 13;
                                break;
                            default:
                                geometry = BarricadeGeometry.Square;
                                break;
                        }

                        break;
                    }
                }

                var newBarricadeBlockPart = new BarricadeBlockPart(Game, geometry, pseudoPoint);
                newBarricadeBlockPart.Attach(this);
                BarricadeBlockParts.Add(newBarricadeBlockPart);
                if (geometry == BarricadeGeometry.LittleRightTriangle) pseudoPoint.X -= 13;
                pseudoPoint.X += blockGap;
            }

            pseudoPoint.X = _point.X;
            pseudoPoint.Y += blockGap;
        }
    }

    public override void Update(GameTime gameTime)
    {
        BarricadeBlockParts.ForEach(e => e.Update(gameTime));
        base.Update(gameTime);
    }

    public void Notify(BarricadeBlockPart part)
    {
        BarricadeBlockParts.Remove(part);
    }

    protected override void Dispose(bool disposing)
    {
        foreach (var barricadeBlockPart in BarricadeBlockParts)
        {
            barricadeBlockPart.Dispose();
        }
        base.Dispose(disposing);
    }
}