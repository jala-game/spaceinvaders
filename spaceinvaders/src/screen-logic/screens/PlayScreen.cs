using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using spaceinvaders.model;
using spaceinvaders.model.aliens;
using spaceinvaders.model.aliens.ships;
using spaceinvaders.model.aliens.ships.queue_aliens;
using spaceinvaders.model.barricades;
using spaceinvaders.model.screens.PlayScreen;
using spaceinvaders.utils;

namespace spaceinvaders.screen_logic.screens;

public class PlayScreen(
    Game game,
    SpaceShip ship,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch)
    : GameScreenModel
{
    private const int InitialTime = 0;
    private readonly Barricades _barricades = new(game);
    private readonly List<IEnemyGroup> _enemies = [];
    private readonly Score _score = new(graphics, spriteBatch, contentManager);
    private int _addLifeManage = 1000;
    private AlienRound _alienRound = new(contentManager, spriteBatch, graphics);
    private Explosion _explosion;
    private int _gameTimeSecond = 0;
    private int _numberOfWaves;
    private RedEnemy _redEnemy;

    public override void LoadContent()
    {
        SoundEffects.LoadMusic(game, ESoundsEffects.BackgroundSong);
        SoundEffects.PlayEffects(true, 0.2f);
    }

    public override void Update(GameTime gameTime)
    {
        if (ship.GetIsDead())
        {
            LoadGameOverScreen();
            return;
        }

        PlayScreenUpdate playScreenUpdate = new()
        {
            Game = game,
            Enemies = _enemies,
            RedEnemy = _redEnemy,
            Ship = ship,
            ContentManager = contentManager,
            Graphics = graphics,
            SpriteBatch = spriteBatch,
            Barricades = _barricades,
            Score = _score,
            AlienRound = _alienRound,
            AddLifeManage = _addLifeManage
        };
        SpawnRedShip(gameTime);
        RemoveRedShip();
        playScreenUpdate.EnemiesUpdate();
        playScreenUpdate.AlienRound.Update();
        playScreenUpdate.EnemyBulletUpdate();
        playScreenUpdate.Ship.Update();
        playScreenUpdate.IncreaseLife();

        var addLifeFromUpdate = playScreenUpdate.AddLifeManage;
        if (addLifeFromUpdate != _addLifeManage) _addLifeManage = addLifeFromUpdate;

        playScreenUpdate.SpaceShipBulletUpdate();
        GenerateNewWaveOfEnemies();
        CollisionEnemyWithSpaceShip();
        playScreenUpdate.Barricades.Update(gameTime);

        var newExplosion = playScreenUpdate.GetExplosion();
        if (newExplosion != null) _explosion = newExplosion;
        _explosion?.Update(gameTime);

        base.Update(gameTime);
    }

    private void LoadGameOverScreen()
    {
        SoundEffects.StopMusic();
        GameOverScreen gameOverScreen = new(game, graphics, contentManager, spriteBatch, _score.GetScore());
        ScreenManager.ChangeScreen(gameOverScreen);
        _barricades.Dispose();
    }

    private void SpawnRedShip(GameTime gameTime)
    {
        var actualSecond = int.Parse(gameTime.TotalGameTime.Seconds.ToString());
        var differenceToSpawnRedShip = actualSecond - InitialTime;
        const int seconds = 59;
        if (differenceToSpawnRedShip == seconds) _redEnemy = new RedEnemy(game, contentManager, spriteBatch, graphics);
    }

    private void RemoveRedShip()
    {
        if (_redEnemy != null && _redEnemy.IsDead()) _redEnemy = null;
    }

    public override void Draw(GameTime gameTime)
    {
        ship.Bullet?.Draw();
        ship.Draw();
        _score.Draw();
        DrawLife();
        DrawEnemies();
        _redEnemy?.Draw();
        DrawWaverText();
        base.Draw(gameTime);
        _explosion?.Draw();
    }

    private void DrawEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Draw();

            var bullet = enemy.GetBullet();
            bullet?.Draw();
        }
    }

    private void DrawLife()
    {
        var spriteFont = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
        spriteBatch.DrawString(spriteFont, $"LIFE {ship.GetLives()}", new Vector2(50, 50), Color.White);
    }

    private void GenerateNewWaveOfEnemies()
    {
        if (_enemies.Count > 0) return;
        _alienRound = new AlienRound(contentManager, spriteBatch, graphics);
        _alienRound.GetEnemies().ForEach(e => { _enemies.Add(e); });
        _numberOfWaves += 1;
    }

    private void DrawWaverText()
    {
        var textWaver = $"Wave {_numberOfWaves}";
        var spriteFont = game.Content.Load<SpriteFont>("fonts/PixeloidMono");
        var textWidth = spriteFont.MeasureString(textWaver).X / 2;

        spriteBatch.DrawString(spriteFont, textWaver,
            new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth, 50), Color.White);
    }

    private void CollisionEnemyWithSpaceShip()
    {
        _enemies.ForEach(e =>
        {
            if (ship.Bounds.Intersects(e.Bounds)) ship.SetIsDead();

            if (e.Bounds.Position.Y < ship.Bounds.Position.Y) return;
            ship.SetIsDead();
        });
    }
}