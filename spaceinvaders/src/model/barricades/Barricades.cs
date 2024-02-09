using Microsoft.Xna.Framework;

namespace spaceinvaders.model
{
    public class Barricades : GameComponent
    {
        private readonly BarricadeBlock[] _barricadeBlocks;
        private const int BarricadeQuantity = 4;

        public Barricades(Game game) : base(game)
        {
            _barricadeBlocks = new BarricadeBlock[BarricadeQuantity];
        }

        public override void Initialize()
        {
            for (int i = 0; i < BarricadeQuantity; i++)
            {
                _barricadeBlocks[i] = new BarricadeBlock(Game, new Point(100, 100));
                Game.Components.Add(_barricadeBlocks[i]);
            }
            base.Initialize();
        }
    }
}