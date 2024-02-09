using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class PlayScreen : GameScreenModel
{

    private readonly SpaceShip spaceShip;
    private readonly GraphicsDeviceManager _graphics;
    private readonly List<IEnemyGroup> enemies = new();
    private RedEnemy _redEnemy;
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

        AlienRound alienRound = new(_contentManager, _spriteBatch, _graphics);
        alienRound.GetEnemies().ForEach(e => enemies.Add(e));
    }

    public override void LoadContent() {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
       this.SpawnRedShip(gameTime);
       this.RemoveRedShip();
       this.EnemiesUpdate();
       this.EnemyBulletUpdate();
       this.spaceShip.Update();
       this.SpaceShipBulletUpdate();
       base.Update(gameTime);
    }

    private void SpawnRedShip(GameTime gameTime) {
        int actualMinute = int.Parse(gameTime.TotalGameTime.Minutes.ToString());
        int differenceToSpawnRedShip = actualMinute - initialTime;
        int MINUTES = 1;
        if (differenceToSpawnRedShip == MINUTES) {
            _redEnemy = new RedEnemy(_contentManager, _spriteBatch, _graphics);
            initialTime = actualMinute;
        }
    }

    private void RemoveRedShip()
    {
        if (_redEnemy !=null && _redEnemy.IsDead())
        {
            _redEnemy = null;
        }
    }

    private void SpaceShipBulletUpdate() {
        if (spaceShip.bullet == null) return;

        spaceShip.bullet.Update();

        foreach (IEnemyGroup enemy in enemies) {
            if (spaceShip.bullet != null && spaceShip.bullet.Bounds.Intersects(enemy.Bounds)) {
                enemy.OnCollision(null);
                spaceShip.OnCollision(null);
            }
        }
        
        enemies.RemoveAll(e => e.IsDead() == true);


        if (_redEnemy == null) return;
        
        bool intersectBetweenRedEnemyAndBullet = spaceShip.bullet.Bounds.Intersects(_redEnemy.Bounds);
        bool spaceShipBulletExists = spaceShip.bullet != null;
        
        if (spaceShipBulletExists && intersectBetweenRedEnemyAndBullet) _redEnemy.OnCollision(null);
        
    }

    private void EnemyBulletUpdate()
    {
        foreach (IEnemyGroup enemy in enemies)
        {
            Bullet bullet = enemy.GetBullet();
            if (bullet != null && bullet.Bounds.Intersects(spaceShip.Bounds))
            {
                bullet.OnCollision(null);
                Console.WriteLine("Matou a nave");
            }
                
        }
    }


    private void EnemiesUpdate() {
        foreach (IEnemyGroup enemy in enemies) {
            enemy.Update();
        }
        _redEnemy?.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        spaceShip.bullet?.Draw();
        this.spaceShip.Draw();
        this.DrawEnemies();
        _redEnemy?.Draw();

        base.Draw(gameTime);
    }

    private void DrawEnemies() {
        foreach (IEnemyGroup enemy in enemies) {
            enemy.Draw();
        }
    }
}