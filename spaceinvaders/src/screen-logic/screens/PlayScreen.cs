using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using spaceinvaders.model;
using spaceinvaders.model.barricades;
using spaceinvaders.model.sounds;

namespace spaceinvaders.screen_logic.screens;

public class PlayScreen(
    Game game,
    SpaceShip ship,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch)
    : GameScreenModel
{
    private readonly List<IEnemyGroup> _enemies = [];
    private RedEnemy _redEnemy;
    private int _initialTime = 0;
    private readonly Score _score = new(graphics, spriteBatch, contentManager);
    private Barricades _barricades = new(game);
    private int _addLifeManage = 1000;
    private int _numberOfHordes = 0;
    private Explosion explosion = null;
    private AlienRound alienRound = new(contentManager, spriteBatch, graphics);

    public override void LoadContent()
    {
        SoundEffects.LoadMusic(game, ESoundsEffects.BackgroundSong);
        SoundEffects.PlayEffects(true, 0.2f);
    }

    public override void Update(GameTime gameTime)
    {
        if (ship.GetIsDead()) {
            LoadGameOverScreen();
            return;
        }

        PlayScreenUpdate playScreenUpdate = new() {
            game=game,
            enemies=_enemies,
            redEnemy=_redEnemy,
            ship=ship,
            contentManager=contentManager,
            graphics=graphics,
            spriteBatch=spriteBatch,
            barricades=_barricades,
            score=_score,
            alienRound=alienRound,
            addLifeManage=_addLifeManage
        };
        SpawnRedShip(gameTime);
        RemoveRedShip();
        playScreenUpdate.EnemiesUpdate();
        playScreenUpdate.alienRound.Update();
        playScreenUpdate.EnemyBulletUpdate();
        playScreenUpdate.ship.Update();
        playScreenUpdate.IncreaseLife();

        int addLifeFromUpdate = playScreenUpdate.addLifeManage;
        if (addLifeFromUpdate != _addLifeManage) _addLifeManage = addLifeFromUpdate;

        playScreenUpdate.SpaceShipBulletUpdate();
        GenerateNewHordeOfEnemies();
        ColisionEnemyWithSpaceShip();
        playScreenUpdate.barricades.Update(gameTime);

        Explosion newExplosion = playScreenUpdate.GetExplosion();
        if (newExplosion != null) explosion = newExplosion;
        explosion?.Update(gameTime);

        base.Update(gameTime);
    }

    private void LoadGameOverScreen() {
        SoundEffects.StopMusic();
        GameOverScreen gameOverScreen = new(game,graphics, contentManager, spriteBatch, _score.GetScore());
        ScreenManager.ChangeScreen(gameOverScreen);
        _barricades.Dispose();
    }

    private void SpawnRedShip(GameTime gameTime)
    {
        int actualSecond = int.Parse(gameTime.TotalGameTime.Seconds.ToString());
        int differenceToSpawnRedShip = actualSecond - _initialTime;
        const int seconds = 59;
        if (differenceToSpawnRedShip == seconds)
        {
            _redEnemy = new RedEnemy(game, contentManager, spriteBatch, graphics);
        }
    }

    private void RemoveRedShip()
    {
        if (_redEnemy != null && _redEnemy.IsDead())
        {
            _redEnemy = null;
        }
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
        SpriteFont spriteFont = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
        spriteBatch.DrawString(spriteFont, $"LIFE {ship.GetLifes()}", new Vector2(50, 50), Color.White);
    }

    private void GenerateNewHordeOfEnemies()
    {
        if (_enemies.Count > 0) return;
        alienRound = new(contentManager, spriteBatch, graphics);
        alienRound.GetEnemies().ForEach(e =>
        {
            _enemies.Add(e);
        });
        _numberOfHordes += 1;
    }

    private void DrawHorderText()
    {
        string textHorder = $"HORDE {_numberOfHordes}";
        SpriteFont spriteFont = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
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

            if (e.Bounds.Position.Y < ship.Bounds.Position.Y) return;
            ship.SetIsDead();
        });
    }
}