using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace spaceinvaders.model.barricades;

public class BarricadeBlock : GameComponent
{
    private int _blockPartRows = 3;
    private int _blockPartCollumns = 4;
    public List<BarricadeBlockPart> BarricadeBlockParts { get; }
    private Point _point;

    public BarricadeBlock(Game game, Point point) : base(game)
    {
        _point = point;
        BarricadeBlockParts = new (_blockPartCollumns * _blockPartRows);
        Game.Components.Add(this);
        Initialize();
    }

    public override void Initialize()
    {
        Point pseudoPoint = _point;
        int blockGap = BarricadeFormatList.GetFormat(BarricadeGeometry.SQUARE).BlockSize;
        int lastColumnIndex = _blockPartCollumns - 1;
        int lastRowIndex = _blockPartRows - 1;

        for (int i = 0; i < _blockPartRows; i++)
        {
            for (int j = 0; j < _blockPartCollumns; j++)
            {
                BarricadeGeometry geometry;

                if (j == 0 && (i == 0 || i == lastColumnIndex))
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
                    pseudoPoint.X += 16;
                }
                else
                {
                    geometry = BarricadeGeometry.SQUARE;
                }

                BarricadeBlockParts.Add(new BarricadeBlockPart(Game, geometry, pseudoPoint));
                if (geometry == BarricadeGeometry.LITTLE_RIGHT_TRIANGLE) pseudoPoint.X -= 16;
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
}