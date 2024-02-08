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
    private readonly List<Entity> entities = new();
    private readonly List<IEnemyEntity> enemies = new();
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
        entities.Add(spaceShip);

        base.Initialize();
    }

    public override void LoadContent() {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
       this.SpawnRedShip(gameTime);
       this.EnemiesUpdate();
       this.EntitiesUpdate();
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
            if (spaceShip.bullet.Bounds.Intersects(enemy.Bounds)) {
                enemy.OnCollision(null);
            }
        }

        enemies.RemoveAll(e => e.IsDead() == true);
    }

    private void EntitiesUpdate() {
        foreach (Entity entity in entities) {
            entity.Update();
        }
    }

    private void EnemiesUpdate() {
        foreach (Entity enemy in enemies) {
            enemy.Update();
        }
    }

    public override void Draw(GameTime gameTime)
    {

        this.DrawSpaceShip();
        this.DrawEntities();

        base.Draw(gameTime);
    }

    private void DrawSpaceShip() {
        spaceShip.bullet?.Draw();
    }

    private void DrawEntities() {
        foreach (Entity entity in entities) {
            entity.Draw();
        }

        foreach (Entity enemy in enemies) {
            enemy.Draw();
        }
    }
}