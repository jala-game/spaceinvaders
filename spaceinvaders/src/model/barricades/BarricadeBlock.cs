using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace spaceinvaders.model.barricades;

public class BarricadeBlock : GameComponent
{
    private const int BlockPartRows = 3;
    private const int BlockPartColumns = 4;
    public List<BarricadeBlockPart> BarricadeBlockParts { get; }
    private readonly Point _point;

    public BarricadeBlock(Game game, Point point) : base(game)
    {
        _point = point;
        BarricadeBlockParts = new();
        Game.Components.Add(this);
        Initialize();
    }

    public override void Initialize()
    {
        Point pseudoPoint = _point;
        int blockGap = BarricadeFormatList.GetFormat(BarricadeGeometry.Square).BlockSize;
        int lastColumnIndex = BlockPartColumns - 1;
        int lastRowIndex = BlockPartRows - 1;

        for (int i = 0; i < BlockPartRows; i++)
        {
            for (int j = 0; j < BlockPartColumns; j++)
            {
                BarricadeGeometry geometry;

                if (j == 0 && (i == 0 || i == lastColumnIndex))
                {
                    geometry = BarricadeGeometry.LeftTriangle;
                }
                else if (j == lastColumnIndex && (i == 0 || i == lastColumnIndex))
                {
                    geometry = BarricadeGeometry.RightTriangle;
                }
                else if (i == lastRowIndex && j == 1)
                {
                    geometry = BarricadeGeometry.LittleLeftTriangle;
                }
                else if (i == lastRowIndex && j == lastColumnIndex - 1)
                {
                    geometry = BarricadeGeometry.LittleRightTriangle;
                    pseudoPoint.X += 13;
                }
                else
                {
                    geometry = BarricadeGeometry.Square;
                }

                BarricadeBlockParts.Add(new BarricadeBlockPart(Game, geometry, pseudoPoint));
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
        BarricadeBlockParts.RemoveAll(a => a.IsBroked);
        base.Update(gameTime);
    }
}