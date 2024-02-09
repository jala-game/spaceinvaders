using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace spaceinvaders.model;

public class BarricadeBlock : GameComponent
{
    private Dictionary<int, BarricadeBlockPart> _barricadeBlockParts;
    private Point _point;

    public BarricadeBlock(Game game, Point point) : base(game)
    {
        _point = point;
        _barricadeBlockParts = new Dictionary<int, BarricadeBlockPart>();
    }

    public override void Initialize()
    {
        for (int i = 0; i < 12; i += 1)
        {
            for (int j = 0; j < 4; j++)
            {
                _barricadeBlockParts[i] = new BarricadeBlockPart(Game, BarricadeGeometry.LEFT_TRIANGLE, _point);
                Game.Components.Add(_barricadeBlockParts[i]);
            }
        }
    }
}