using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

public class PlayScreen : GameScreenModel
{

    private readonly SpaceShip spaceShip;
    private readonly GraphicsDeviceManager _graphics;
    public readonly List<IEnemyEntity> enemies = new();
    private readonly ContentManager _contentManager;
    private readonly SpriteBatch _spriteBatch;
    private int initialTime = 0;

    public PlayScreen(SpaceShip ship, GraphicsDeviceManager graphics, ContentManager contentManager, SpriteBatch spriteBatch)
    {
        spaceShip = ship;
        _graphics = graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
    }

    public override void Initialize() {
        base.Initialize();

        AlienQueue queue = new(_contentManager, _spriteBatch, _graphics, AlienEnum.SHOOTER, 100);
        AlienQueue queue1 = new(_contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, 100 + 70);
        AlienQueue queue2 = new(_contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, 100 + 70 * 2);
        AlienQueue queue3 = new(_contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, 100 + 70 * 3);
        AlienQueue queue4 = new(_contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, 100 + 70 * 4);


        foreach (IEnemyEntity enemy in queue.GetEnemies()) {
            enemies.Add(enemy);
        }

        foreach (IEnemyEntity enemy in queue1.GetEnemies()) {
            enemies.Add(enemy);
        }
                foreach (IEnemyEntity enemy in queue2.GetEnemies()) {
            enemies.Add(enemy);
        }        foreach (IEnemyEntity enemy in queue3.GetEnemies()) {
            enemies.Add(enemy);
        }        foreach (IEnemyEntity enemy in queue4.GetEnemies()) {
            enemies.Add(enemy);
        }      

        enemies.Add(queue);
        enemies.Add(queue1);
        enemies.Add(queue2);
        enemies.Add(queue3);
        enemies.Add(queue4);
    }

    public override void LoadContent() {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
       this.SpawnRedShip(gameTime);
       this.EnemiesUpdate();
       this.spaceShip.Update();
       this.SpaceShipBulletUpdate();
       base.Update(gameTime);
    }

    private void SpawnRedShip(GameTime gameTime) {
        int actualMinute = int.Parse(gameTime.TotalGameTime.Minutes.ToString());
        int differenceToSpawnRedShip = actualMinute - initialTime;
        int MINUTES = 1;
        if (differenceToSpawnRedShip == MINUTES) {
            enemies.Add(new RedEnemy(_contentManager, _spriteBatch, _graphics ));
            initialTime = actualMinute;
        }
    }

    private void SpaceShipBulletUpdate() {
        if (spaceShip.bullet == null) return;

        spaceShip.bullet.Update();

        foreach (Entity enemy in enemies) {
            if (spaceShip.bullet != null && spaceShip.bullet.Bounds.Intersects(enemy.Bounds)) {
                enemy.OnCollision(null);
                spaceShip.OnCollision(null);
            }
        }

        enemies.RemoveAll(e => e.IsDead() == true);
    }


    private void EnemiesUpdate() {
        foreach (Entity enemy in enemies) {
            enemy.Update();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        spaceShip.bullet?.Draw();
        this.spaceShip.Draw();
        this.DrawEnemies();

        base.Draw(gameTime);
    }

    private void DrawEnemies() {
        foreach (Entity enemy in enemies) {
            enemy.Draw();
        }
    }
}