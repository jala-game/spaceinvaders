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
    private readonly bool isDead = false;
    private float SPEED = 1f;

    public AlienQueue(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, AlienEnum enemyType, int y=200) {
        _graphics = graphics;
        _spriteBatch = spriteBatch;

        int ENEMY_LIMIT = 11;
        int MARGIN = 80;
        switch (enemyType) {
            case AlienEnum.SHOOTER:
                for (int i = 1; i <= ENEMY_LIMIT; i++) {
                    enemies.Add(new ShooterEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                }
                break;

            case AlienEnum.BIRD:
                for (int i = 1; i <= ENEMY_LIMIT; i++) {
                    enemies.Add(new BirdEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                }
                break;
            case AlienEnum.FRONT:
                for (int i = 1; i <= ENEMY_LIMIT; i++) {
                    enemies.Add(new FrontEnemy(contentManager, spriteBatch, _graphics, i * MARGIN, y));
                }
                break;
        }
    }

    public void Update()
    {
        foreach (IEnemyGroup enemy in enemies) {
            enemy.IncreaseX(SPEED);

            bool rightLimit = enemy.Bounds.Position.X + enemy.GetTexture().Width >= _graphics.PreferredBackBufferWidth;
            bool leftLimit = enemy.Bounds.Position.X <= 0;
            if (rightLimit || leftLimit) {
                enemies.ForEach(e => {
                    e.InvertDirection();
                    e.Fall();
                });
                SPEED+= 0.2f;
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