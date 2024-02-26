using MonoGame.Extended.Collisions;

namespace spaceinvaders.model;

public interface IEntity : ICollisionActor
{
    public void Update();
    public void Draw();
}