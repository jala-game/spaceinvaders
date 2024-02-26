using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.enums;
using spaceinvaders.model.aliens.ships;
using spaceinvaders.model.aliens.ships.queue_aliens;

public class AlienQueue : IEnemyEntity
{
    private const bool isDead = false;
    private readonly List<IEnemyGroup> _enemies = [];
    private readonly SpriteBatch _spriteBatch;

    public AlienQueue(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,
        AlienEnum enemyType, int y = 200)
    {
        _spriteBatch = spriteBatch;

        const int enemyLimit = 11;
        const int margin = 80;
        switch (enemyType)
        {
            case AlienEnum.SHOOTER:
                for (var i = 1; i <= enemyLimit; i++)
                    _enemies.Add(new ShooterEnemy(contentManager, spriteBatch, graphics, i * margin, y));
                break;

            case AlienEnum.BIRD:
                for (var i = 1; i <= enemyLimit; i++)
                    _enemies.Add(new BirdEnemy(contentManager, spriteBatch, graphics, i * margin, y));
                break;
            case AlienEnum.FRONT:
                for (var i = 1; i <= enemyLimit; i++)
                    _enemies.Add(new FrontEnemy(contentManager, spriteBatch, graphics, i * margin, y));
                break;
        }
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
    }

    public void Draw()
    {
    }

    public bool IsDead()
    {
        return isDead;
    }

    public IEnumerable<IEnemyGroup> GetEnemies()
    {
        return _enemies;
    }
}