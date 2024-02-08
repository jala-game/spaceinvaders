using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienQueue : IEnemyEntity
{
    public IShapeF Bounds { get; }
    private readonly List<IEnemyGroup> enemies = new();
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
                    int MARGIN = 80;
                    enemies.Add(new ShooterEnemy(contentManager, spriteBatch, _graphics, i * MARGIN));
                }
                break;
        }
    }

    public void Update()
    {
        foreach (IEnemyGroup enemy in enemies) {
            enemy.IncreaseX(1);

            if (enemy.Bounds.Position.X + enemy.GetTexture().Width >= _graphics.PreferredBackBufferWidth) {
                enemies.ForEach(e => e.SetInitialPosition());
            }
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

    public List<IEnemyGroup> GetEnemies() {
        return enemies;
    }
}