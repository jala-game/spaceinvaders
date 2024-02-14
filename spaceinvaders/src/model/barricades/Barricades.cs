using Microsoft.Xna.Framework;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model;

public class Barricades : GameComponent
{
    public BarricadeBlock[] BarricadeBlocks { get; }
    private const int BarricadeQuantity = 4;

    public Barricades(Game game) : base(game)
    {
        BarricadeBlocks = new BarricadeBlock[BarricadeQuantity];
        Game.Components.Add(this);
        Initialize();
    }

    public override void Initialize()
    {
        GraphicsDeviceManager gdm = Game.Services.GetService<GraphicsDeviceManager>();
        int totalGapWidth = 100 * (BarricadeQuantity - 1);
        int availableWidth = gdm.PreferredBackBufferWidth - totalGapWidth;
        int barricadeWidth = availableWidth / BarricadeQuantity;

        int startPointX = (gdm.PreferredBackBufferWidth - availableWidth) / 2;
        int startPointY = gdm.PreferredBackBufferHeight - 300;
        var barricadeBlockPoint = new Point(startPointX, startPointY);

        for (int i = 0; i < BarricadeQuantity; i++)
        {
            BarricadeBlocks[i] = new BarricadeBlock(Game, barricadeBlockPoint);
            barricadeBlockPoint.X += barricadeWidth + 100;
        }

        base.Initialize();
    }
}