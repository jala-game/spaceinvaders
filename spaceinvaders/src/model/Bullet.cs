using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class Bullet : ICollisionActor
{
    public IShapeF Bounds => throw new System.NotImplementedException();

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        throw new System.NotImplementedException();
    }
}