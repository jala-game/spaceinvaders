using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model.barricades;

public class PlayScreenUpdate() {
    public List<IEnemyGroup> enemies;
    public RedEnemy redEnemy;
    public SpaceShip ship;
    public GraphicsDeviceManager graphics;
    public ContentManager contentManager;
    public SpriteBatch spriteBatch;
    public Barricades barricades;
    private Explosion explosion = null;

    public void EnemiesUpdate() {
        foreach (IEnemyGroup enemy in enemies)
        {
            enemy.Update();
        }

        redEnemy?.Update();
    }

    public void ExplosionUpdate(GameTime gameTime) {
        explosion?.Update(gameTime);
    }

    public void EnemyBulletUpdate()
    {
        foreach (IEnemyGroup enemy in enemies)
        {
            Bullet bullet = enemy.GetBullet();
            bullet?.Update();

            if (bullet != null && bullet.Bounds.Intersects(ship.Bounds))
            {
                bullet.OnCollision(null);
                ship.OnCollision(null);
                explosion = new(spriteBatch, contentManager, ship.Bounds.Position);
            }

            if (bullet != null && bullet.Bounds.Intersects(ship.bullet?.Bounds))
            {
                bullet.OnCollision(null);
                ship.bullet.OnCollision(null);
            }

            if (bullet != null)
            {
                CollisionBulletAndBarricades(bullet);
            }
        }
    }

    private void CollisionBulletAndBarricades(ICollisionActor bullet)
    {
        ArgumentNullException.ThrowIfNull(bullet);
        foreach (var blockPart in barricades.BarricadeBlocks.SelectMany(barricadeBlock =>
                     barricadeBlock.BarricadeBlockParts))
        {
            if (!blockPart.Bounds.Intersects(bullet.Bounds)) continue;
            blockPart.OnCollision(null);
            bullet.OnCollision(null);
            break;
        }
    }
}