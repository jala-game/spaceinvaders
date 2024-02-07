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

    public PlayScreen(SpaceShip ship, GraphicsDeviceManager graphics, ContentManager contentManager, SpriteBatch spriteBatch)
    {
        spaceShip = ship;
        _graphics = graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
    }

    public override void Initialize() {
        entities.Add(spaceShip);
        enemies.Add(new RedEnemy(_contentManager, _spriteBatch, _graphics ));

        base.Initialize();
    }

    public override void LoadContent() {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
       this.EnemiesUpdate();
       this.EntitiesUpdate();
       this.SpaceShipBulletUpdate();
 
       base.Update(gameTime);
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