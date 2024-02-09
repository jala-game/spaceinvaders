using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using spaceinvaders.model;

public class PlayScreen : GameScreenModel
{
    private readonly Barricades _barricades;
    private readonly ContentManager _contentManager;
    private readonly List<IEnemyEntity> _enemies = new();
    private readonly GraphicsDeviceManager _graphics;

    private readonly SpaceShip _spaceShip;
    private readonly SpriteBatch _spriteBatch;

    private int initialTime;

    public PlayScreen(SpaceShip ship, GraphicsDeviceManager graphics, ContentManager contentManager,
        SpriteBatch spriteBatch)
    {
        _spaceShip = ship;
        _graphics = graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
    }

    public override void Initialize()
    {
        base.Initialize();

        AlienRound alienRound = new(_contentManager, _spriteBatch, _graphics);
        alienRound.GetEnemies().ForEach(e => _enemies.Add(e));
    }

    //TODO: Dá para fazer um Observer ness caso aqui de ação do Update, em vez de chamar um por um.
    public override void Update(GameTime gameTime)
    {
        SpawnRedShip(gameTime);
        EnemiesUpdate();
        _spaceShip.Update();
        SpaceShipBulletUpdate();
    }

    private void SpawnRedShip(GameTime gameTime)
    {
        var actualMinute = int.Parse(gameTime.TotalGameTime.Minutes.ToString());
        var differenceToSpawnRedShip = actualMinute - initialTime;
        var MINUTES = 1;
        if (differenceToSpawnRedShip == MINUTES)
        {
            _enemies.Add(new RedEnemy(_contentManager, _spriteBatch, _graphics));
            initialTime = actualMinute;
        }
    }

    private void SpaceShipBulletUpdate()
    {
        if (_spaceShip.bullet == null) return;

        _spaceShip.bullet.Update();

        foreach (var enemy in _enemies)
            if (_spaceShip.bullet != null && _spaceShip.bullet.Bounds.Intersects(enemy.Bounds))
            {
                enemy.OnCollision(null);
                _spaceShip.OnCollision(null);
            }

        _enemies.RemoveAll(e => e.IsDead());
    }

    private void EnemiesUpdate()
    {
        foreach (Entity enemy in _enemies) enemy.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        _spaceShip.bullet?.Draw();
        _spaceShip.Draw();
        DrawEnemies();
        base.Draw(gameTime);
    }

    private void DrawEnemies()
    {
        foreach (Entity enemy in _enemies) enemy.Draw();
    }
}