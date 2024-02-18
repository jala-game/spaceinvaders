using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;
using spaceinvaders.model;
using spaceinvaders.model.barricades;

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
    private readonly Score _score = new(graphics, spriteBatch, contentManager);
    private Barricades _barricades = new(game);
    private int _addLifeManage = 1000;
    private int _numberOfHordes = 0;
    private Explosion explosion = null;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        if (ship.GetIsDead()) {
            LoadGameOverScreen();
            return;
        }
        SpawnRedShip(gameTime);
        RemoveRedShip();
        EnemiesUpdate();
        EnemiesLogicUpdate();
        EnemyBulletUpdate();
        ship.Update();
        UpdateLife();
        SpaceShipBulletUpdate();
        GenerateNewHordeOfEnemies();
        ColisionEnemyWithSpaceShip();
        UpdateBarricades(gameTime);
        explosion?.Update(gameTime);
        base.Update(gameTime);
    }

    private void LoadGameOverScreen() {
        GameOverScreen gameOverScreen = new(graphics, contentManager, spriteBatch, _score.GetScore());
        ScreenManager.ChangeScreen(gameOverScreen);
        return;
    }

    private void CollisionBulletAndBarricades(Bullet bullet)
    {
        foreach (var blockPart in _barricades.BarricadeBlocks.SelectMany(barricadeBlock =>
                     barricadeBlock.BarricadeBlockParts))
        {
            if (blockPart.Bounds.Intersects(bullet.Bounds))
            {
                blockPart.OnCollision(null);
                bullet.OnCollision(null);
            }
        }
    }

    private void EnemiesLogicUpdate()
    {
        _groupLogics.ForEach(e => e.Update());
    }

    private void UpdateBarricades(GameTime gameTime)
    {
        _barricades.Update(gameTime);
    }

    private void UpdateLife()
    {
        if (_score.GetScore() >= _addLifeManage)
        {
            _addLifeManage += 1000;
            ship.AddLifeForShip();
        }
    }

    private void SpawnRedShip(GameTime gameTime)
    {
        int actualMinute = int.Parse(gameTime.TotalGameTime.Minutes.ToString());
        int differenceToSpawnRedShip = actualMinute - _initialTime;
        const int minutes = 1;
        if (differenceToSpawnRedShip == minutes)
        {
            _redEnemy = new RedEnemy(contentManager, spriteBatch, graphics);
            _initialTime = actualMinute;
        }
    }

    private void RemoveRedShip()
    {
        if (_redEnemy != null && _redEnemy.IsDead())
        {
            _redEnemy = null;
        }
    }

    private void SpaceShipBulletUpdate()
    {
        if (ship.bullet == null) return;

        ship.bullet.Update();

        foreach (IEnemyGroup enemy in _enemies)
        {
            if (ship.bullet != null && ship.bullet.Bounds.Intersects(enemy.Bounds))
            {
                _score.SetScore(enemy.GetPoint());
                enemy.OnCollision(null);
                ship.bullet.OnCollision(null);
                explosion = new(spriteBatch, contentManager, enemy.Bounds.Position);
            }
        }

        _enemies.RemoveAll(e => e.IsDead());

        if (ship.bullet != null)
        {
            CollisionBulletAndBarricades(ship.bullet);
        }

        if (_redEnemy == null) return;

        bool intersectBetweenRedEnemyAndBullet = ship.bullet.Bounds.Intersects(_redEnemy.Bounds);
        bool spaceShipBulletExists = ship.bullet != null;

        if (spaceShipBulletExists && intersectBetweenRedEnemyAndBullet)
        {
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
                explosion = new(spriteBatch, contentManager, ship.Bounds.Position);
            }

            if (bullet != null)
            {
                CollisionBulletAndBarricades(bullet);
            }
        }
    }


    private void EnemiesUpdate()
    {
        foreach (IEnemyGroup enemy in _enemies)
        {
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
        DrawHorderText();
        base.Draw(gameTime);
        explosion?.Draw();
    }

    private void DrawEnemies()
    {
        foreach (IEnemyGroup enemy in _enemies)
        {
            enemy.Draw();

            Bullet bullet = enemy.GetBullet();
            bullet?.Draw();
        }
    }

    private void DrawLife()
    {
        SpriteFont spriteFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
        spriteBatch.DrawString(spriteFont, $"LIFE {ship.GetLifes()}", new Vector2(50, 50), Color.White);
    }

    private void GenerateNewHordeOfEnemies()
    {
        if (_enemies.Count > 0) return;
        _groupLogics.Clear();
        AlienRound alienRound = new(contentManager, spriteBatch, graphics);
        alienRound.GetEnemies().ForEach(e =>
        {
            _enemies.Add(e);
        });
        alienRound.GetLogics().ForEach(e => _groupLogics.Add(e));
        _numberOfHordes += 1;
    }

    private void DrawHorderText()
    {
        string textHorder = $"HORDE {_numberOfHordes}";
        SpriteFont spriteFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
        float textWidth = spriteFont.MeasureString(textHorder).X / 2;

        spriteBatch.DrawString(spriteFont, textHorder,
            new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth , 50), Color.White);
    }

    private void ColisionEnemyWithSpaceShip()
    {
        _enemies.ForEach(e =>
        {
            if (ship.Bounds.Intersects(e.Bounds))
            {
                ship.SetIsDead();
            }
        });
    }
}