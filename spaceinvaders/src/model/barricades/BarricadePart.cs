using System;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace spaceinvaders.model;

public abstract class BarricadePart : Entity
{
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        throw new NotImplementedException();
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Draw()
    {
        throw new NotImplementedException();
    }
}