using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;

public class PlayScreen : GameScreenModel
{
    private readonly ContentManager _contentManager;
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly List<IEnemyEntity> _enemies = new();

    private readonly SpaceShip spaceShip;
    private int initialTime;

    public PlayScreen(SpaceShip ship, GraphicsDeviceManager graphics, ContentManager contentManager,
        SpriteBatch spriteBatch)
    {
        spaceShip = ship;
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

    //TODO: Dá para fazer um Observer ness caso aqui, em vez de chamar um por um
    public override void Update(GameTime gameTime)
    {
        SpawnRedShip(gameTime);
        EnemiesUpdate();
        spaceShip.Update();
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
        if (spaceShip.bullet == null) return;

        spaceShip.bullet.Update();

        foreach (IEnemyEntity enemy in _enemies)
            if (spaceShip.bullet != null && spaceShip.bullet.Bounds.Intersects(enemy.Bounds))
            {
                enemy.OnCollision(null);
                spaceShip.OnCollision(null);
            }

        _enemies.RemoveAll(e => e.IsDead());
    }

    private void EnemiesUpdate()
    {
        foreach (Entity enemy in _enemies) enemy.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        spaceShip.bullet?.Draw();
        spaceShip.Draw();
        DrawEnemies();

        base.Draw(gameTime);
    }

    private void DrawBarricades()
    {

    }

    private void DrawEnemies()
    {
        foreach (Entity enemy in _enemies) enemy.Draw();
    }
}