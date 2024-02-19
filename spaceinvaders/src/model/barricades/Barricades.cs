using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace spaceinvaders.model.barricades;

public class Barricades : GameComponent
{
    public List<BarricadeBlock> BarricadeBlocks { get; }
    private const int BarricadeQuantity = 4;

    public Barricades(Game game) : base(game)
    {
        BarricadeBlocks = new List<BarricadeBlock>(BarricadeQuantity);
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        var gdm = Game.Services.GetService<GraphicsDeviceManager>();
        const int totalGapWidth = 100 * (BarricadeQuantity - 1);
        var availableWidth = gdm.PreferredBackBufferWidth - totalGapWidth;
        var barricadeWidth = availableWidth / BarricadeQuantity;

        var startPointX = (gdm.PreferredBackBufferWidth - availableWidth) / 2;
        var startPointY = gdm.PreferredBackBufferHeight - 300;
        var barricadeBlockPoint = new Point(startPointX, startPointY);

        for (var i = 0; i < BarricadeQuantity; i++)
        {
            BarricadeBlocks.Add(new BarricadeBlock(Game, barricadeBlockPoint));
            barricadeBlockPoint.X += barricadeWidth + 100;
        }
    }

    public override void Update(GameTime gameTime)
    {
        BarricadeBlocks.ForEach(e => e.Update(gameTime));
        base.Update(gameTime);
    }
}