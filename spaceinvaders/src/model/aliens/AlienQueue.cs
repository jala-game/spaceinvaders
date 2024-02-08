using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienQueue : IEnemyEntity
{
    public IShapeF Bounds { get; }
    private readonly List<IEnemyEntity> enemies = new();
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool isDead = false;

    public AlienQueue(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, AlienEnum enemyType) {
        _graphics = graphics;
        _spriteBatch = spriteBatch;

        switch (enemyType) {
            case AlienEnum.SHOOTER:
                int ENEMY_LIMIT = 11;
                for (int i = 1; i <= ENEMY_LIMIT; i++) {
                    enemies.Add(new ShooterEnemy(contentManager, spriteBatch, _graphics, i * 100));
                }
                break;
        }
    }

    public void Update()
    {
        foreach (IEnemyEntity enemy in enemies) {
            enemy.IncreaseX(1);
        }
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        throw new System.NotImplementedException();
    }

    public void Draw()
    {
    }

    public bool IsDead()
    {
        return isDead;
    }

    public List<IEnemyEntity> GetEnemies() {
        return enemies;
    }

    public void IncreaseX(int x)
    {
        throw new System.NotImplementedException();
    }
}