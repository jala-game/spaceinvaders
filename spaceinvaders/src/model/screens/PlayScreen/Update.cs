using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model.aliens;
using spaceinvaders.model.aliens.ships;
using spaceinvaders.model.aliens.ships.queue_aliens;
using spaceinvaders.model.barricades;
using spaceinvaders.utils;

namespace spaceinvaders.model.screens.PlayScreen;

public class PlayScreenUpdate
{
    public int AddLifeManage;
    public AlienRound AlienRound;
    public Barricades Barricades;
    public ContentManager ContentManager;
    public List<IEnemyGroup> Enemies;
    private Explosion _explosion;
    public Game Game;
    public GraphicsDeviceManager Graphics;
    public RedEnemy RedEnemy;
    public Score Score;
    public SpaceShip Ship;
    public SpriteBatch SpriteBatch;

    public void EnemiesUpdate()
    {
        foreach (var enemy in Enemies) enemy.Update();

        RedEnemy?.Update();
    }

    public Explosion GetExplosion()
    {
        return _explosion;
    }

    public void EnemyBulletUpdate()
    {
        foreach (var bullet in Enemies.Select(enemy => enemy.GetBullet()))
        {
            bullet?.Update();

            if (bullet != null && bullet.Bounds.Intersects(Ship.Bounds))
            {
                bullet.OnCollision(null);
                Ship.OnCollision(null);
                _explosion = new Explosion(SpriteBatch, ContentManager, Ship.Bounds.Position);
            }

            if (bullet != null && bullet.Bounds.Intersects(Ship.Bullet?.Bounds))
            {
                bullet.OnCollision(null);
                Ship.Bullet.OnCollision(null);
            }

            if (bullet != null) CollisionBulletAndBarricades(bullet);
        }
    }

    public void SpaceShipBulletUpdate()
    {
        if (Ship.Bullet == null) return;

        Ship.Bullet.Update();

        foreach (var enemy in Enemies.Where(enemy => Ship.Bullet != null && Ship.Bullet.Bounds.Intersects(enemy.Bounds)))
        {
            Score.SetScore(enemy.GetPoint());
            enemy.OnCollision(null);
            Ship.Bullet.OnCollision(null);
            _explosion = new Explosion(SpriteBatch, ContentManager, enemy.Bounds.Position);
            SoundEffects.LoadEffect(Game, ESoundsEffects.EnemyDead);
            SoundEffects.PlaySoundEffect();
        }

        Enemies.RemoveAll(e => e.IsDead());

        if (Ship.Bullet != null) CollisionBulletAndBarricades(Ship.Bullet);

        if (RedEnemy == null) return;

        var intersectBetweenRedEnemyAndBullet = Ship.Bullet.Bounds.Intersects(RedEnemy.Bounds);
        var spaceShipBulletExists = Ship.Bullet != null;

        if (!spaceShipBulletExists || !intersectBetweenRedEnemyAndBullet) return;
        Score.SetScore(RedEnemy.GetPoint());
        RedEnemy.OnCollision(null);
        _explosion = new Explosion(SpriteBatch, ContentManager, RedEnemy.Bounds.Position);
        SoundEffects.LoadEffect(Game, ESoundsEffects.EnemyDead);
        SoundEffects.PlaySoundEffect();
    }

    private void CollisionBulletAndBarricades(ICollisionActor bullet)
    {
        ArgumentNullException.ThrowIfNull(bullet);
        foreach (var blockPart in Barricades.BarricadeBlocks.SelectMany(barricadeBlock =>
                     barricadeBlock.BarricadeBlockParts))
        {
            if (!blockPart.Bounds.Intersects(bullet.Bounds)) continue;
            blockPart.OnCollision(null);
            bullet.OnCollision(null);
            break;
        }
    }

    public void IncreaseLife()
    {
        if (Score.GetScore() < AddLifeManage) return;
        AddLifeManage += 1000;
        Ship.AddLifeForShip();
    }
}