using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace spaceinvaders.model.barricades;

public class BarricadeBlock : GameComponent
{
    private int _blockPartRows = 3;
    private int _blockPartCollumns = 4;
    public Dictionary<int, BarricadeBlockPart> BarricadeBlockParts { get; }
    private Point _point;

    public BarricadeBlock(Game game, Point point) : base(game)
    {
        _point = point;
        BarricadeBlockParts = new Dictionary<int, BarricadeBlockPart>();
        Game.Components.Add(this);
        Initialize();
    }

    public override void Initialize()
    {
        Point pseudoPoint = _point;
        int blockIndex = 0;
        int blockGap = BarricadeFormatList.GetFormat(BarricadeGeometry.SQUARE).BlockSize;
        int lastColumnIndex = _blockPartCollumns - 1;
        int lastRowIndex = _blockPartRows - 1;

        for (int i = 0; i < _blockPartRows; i++)
        {
            for (int j = 0; j < _blockPartCollumns; j++)
            {
                BarricadeGeometry geometry;

                if (j == 0  && (i == 0 || i == lastColumnIndex))
                {
                    geometry = BarricadeGeometry.LEFT_TRIANGLE;
                }
                else if (j == lastColumnIndex && (i == 0 || i == lastColumnIndex))
                {
                    geometry = BarricadeGeometry.RIGHT_TRIANGLE;
                }
                else if (i == lastRowIndex && j == 1)
                {
                    geometry = BarricadeGeometry.LITTLE_LEFT_TRIANGLE;
                }
                else if (i == lastRowIndex && j == lastColumnIndex - 1)
                {
                    geometry = BarricadeGeometry.LITTLE_RIGHT_TRIANGLE;
                }
                else
                {
                    geometry = BarricadeGeometry.SQUARE;
                }

                BarricadeBlockParts[blockIndex] = new BarricadeBlockPart(Game, geometry, pseudoPoint);
                pseudoPoint.X += blockGap;
                blockIndex++;
            }

            pseudoPoint.X = _point.X;
            pseudoPoint.Y += blockGap;
        }
    }
}