using MonoGame.Extended.Collisions;

namespace spaceinvaders.model;

public interface Entity : ICollisionActor, Sprite
{
    public void Update();
    public void Draw();
}