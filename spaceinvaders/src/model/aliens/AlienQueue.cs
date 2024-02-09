using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienQueue : IEnemyEntity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private readonly List<IEnemyGroup> enemies = new();
    private readonly bool isDead = false;

    public AlienQueue(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,
        AlienEnum enemyType, int y = 200)
    {
        _graphics = graphics;
        _spriteBatch = spriteBatch;

        var ENEMY_LIMIT = 11;
        var MARGIN = 80;
        switch (enemyType)
        {
            case AlienEnum.SHOOTER:
                for (var i = 1; i <= ENEMY_LIMIT; i++)
                    enemies.Add(new ShooterEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                break;

            case AlienEnum.BIRD:
                for (var i = 1; i <= ENEMY_LIMIT; i++)
                    enemies.Add(new BirdEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                break;
            case AlienEnum.FRONT:
                for (var i = 1; i <= ENEMY_LIMIT; i++)
                    enemies.Add(new FrontEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                break;
        }
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        foreach (var enemy in enemies)
        {
            enemy.IncreaseX(1);

            var rightLimit = enemy.Bounds.Position.X + enemy.GetTexture().Width >= _graphics.PreferredBackBufferWidth;
            var leftLimit = enemy.Bounds.Position.X <= 0;
            if (rightLimit || leftLimit)
                enemies.ForEach(e =>
                {
                    e.InvertDirection();
                    e.Fall();
                });
        }
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        //
    }

    public void Draw()
    {
        //
    }

    public bool IsDead()
    {
        return isDead;
    }

    public List<IEnemyGroup> GetEnemies()
    {
        return enemies;
    }
}