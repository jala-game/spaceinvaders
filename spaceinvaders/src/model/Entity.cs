using MonoGame.Extended.Collisions;

public interface Entity : ICollisionActor
{
    public void Update();
    public void Draw();
}