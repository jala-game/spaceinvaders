using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using spaceinvaders.model;

namespace spaceinvaders.screen_logic.screens;

public class PlayScreen(
    Game game,
    SpaceShip ship,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch)
    : GameScreenModel
{
    private readonly List<IEnemyGroup> _enemies = new();
    private readonly List<IEnemyEntity> _groupLogics = new();
    private RedEnemy _redEnemy;
    private int _initialTime;
    private readonly Score _score = new(graphics, spriteBatch,contentManager);
    private readonly Barricades _barricades = new(game);
    private int _addLifeManage = 1000;

    public override void Initialize() {
        base.Initialize();

        AlienRound alienRound = new(contentManager, spriteBatch, graphics);
        alienRound.GetEnemies().ForEach(e => _enemies.Add(e));
        alienRound.GetLogics().ForEach(e => _groupLogics.Add(e));
    }

    public override void Update(GameTime gameTime)
    {
        if (ship.GetIsDead() || _score.GetScore() >= 500) return;
        SpawnRedShip(gameTime);
        RemoveRedShip();
        EnemiesUpdate();
        EnemiesLogicUpdate();
        EnemyBulletUpdate();
        ship.Update();
        UpdateLife();
        SpaceShipBulletUpdate();
        base.Update(gameTime);
    }

    private void EnemiesLogicUpdate()
    {
        _groupLogics.ForEach(e => e.Update());
    }

    private void UpdateLife()
    {
        if (_score.GetScore() >= _addLifeManage)
        {
            _addLifeManage += 1000;
            ship.AddLifeForShip();
        }
    }

    private void SpawnRedShip(GameTime gameTime) {
        int actualMinute = int.Parse(gameTime.TotalGameTime.Minutes.ToString());
        int differenceToSpawnRedShip = actualMinute - _initialTime;
        const int minutes = 1;
        if (differenceToSpawnRedShip == minutes) {
            _redEnemy = new RedEnemy(contentManager, spriteBatch, graphics);
            _initialTime = actualMinute;
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
        if (ship.bullet == null) return;

        ship.bullet.Update();

        foreach (IEnemyGroup enemy in _enemies) {
            if (ship.bullet != null && ship.bullet.Bounds.Intersects(enemy.Bounds)) {
                _score.SetScore(enemy.GetPoint());
                enemy.OnCollision(null);
                ship.bullet.OnCollision(null);
            }
        }

        _enemies.RemoveAll(e => e.IsDead());


        if (_redEnemy == null) return;

        bool intersectBetweenRedEnemyAndBullet = ship.bullet.Bounds.Intersects(_redEnemy.Bounds);
        bool spaceShipBulletExists = ship.bullet != null;

        if (spaceShipBulletExists && intersectBetweenRedEnemyAndBullet){
            _score.SetScore(_redEnemy.GetPoint());
            _redEnemy.OnCollision(null);
        }
    }

    private void EnemyBulletUpdate()
    {
        foreach (IEnemyGroup enemy in _enemies)
        {
            Bullet bullet = enemy.GetBullet();
            bullet?.Update();

            if (bullet != null && bullet.Bounds.Intersects(ship.Bounds))
            {
                bullet.OnCollision(null);
                ship.OnCollision(null);
            }
        }
    }


    private void EnemiesUpdate() {
        foreach (IEnemyGroup enemy in _enemies) {
            enemy.Update();
        }
        _redEnemy?.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        ship.bullet?.Draw();
        ship.Draw();
        _score.Draw();
        DrawLife();
        DrawEnemies();
        _redEnemy?.Draw();
        base.Draw(gameTime);
    }

    private void DrawEnemies() {
        foreach (IEnemyGroup enemy in _enemies) {
            enemy.Draw();

            Bullet bullet = enemy.GetBullet();
            bullet?.Draw();
        }
    }

    private void DrawLife()
    {
        SpriteFont spriteFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
        spriteBatch.DrawString(spriteFont, $"Lives {ship.GetLifes()}", new Vector2( 50, 50), Color.White);
    }
}