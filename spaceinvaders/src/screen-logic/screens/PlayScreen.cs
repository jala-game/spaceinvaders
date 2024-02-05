using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PlayScreen : GameScreenModel
{

    private readonly SpaceShip spaceShip;

    public PlayScreen(SpaceShip ship)
    {
        spaceShip = ship;
    }

    public override void Initialize()
    {
        base.Initialize();
    }


    public override void LoadContent() {
        base.Update();
    }

    public override void Update()
    {
        spaceShip.Update();
        base.Update();
    }

    public override void Draw()
    {
        spaceShip.Draw();
        base.Draw();
    }

}