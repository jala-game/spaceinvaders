using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienQueue : Entity
{
    public IShapeF Bounds { get; }
    private readonly List<IEnemyEntity> enemies = new();

    public void Draw()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}