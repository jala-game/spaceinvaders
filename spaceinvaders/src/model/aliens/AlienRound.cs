using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienRound : IEnemyEntity
{
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;

    public AlienRound(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
        _graphics = graphics;
        _spriteBatch = spriteBatch;

        AlienQueue queue = new(contentManager, _spriteBatch, _graphics, AlienEnum.SHOOTER, 100);
        AlienQueue queue1 = new(contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, 100 + 70);
        AlienQueue queue2 = new(contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, 100 + 70 * 2);
        AlienQueue queue3 = new(contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, 100 + 70 * 3);
        AlienQueue queue4 = new(contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, 100 + 70 * 4);
    }

    public void Draw()
    {
        throw new System.NotImplementedException();
    }

    public bool IsDead()
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