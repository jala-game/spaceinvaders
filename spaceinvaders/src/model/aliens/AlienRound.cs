using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using spaceinvaders.enums;
using spaceinvaders.model.aliens.ships;
using spaceinvaders.model.aliens.ships.queue_aliens;

namespace spaceinvaders.model.aliens;

public class AlienRound
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly List<IEnemyGroup> _enemies = [];
    private readonly List<IEnemyEntity> _logics = [];
    private float _speed = 1f;


    public AlienRound(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        _graphics = graphics;
        const int originalPosition = 100;
        const int marginBottom = 70;
        AlienQueue shooterQueue =
            new(contentManager, spriteBatch, _graphics, AlienEnum.SHOOTER, originalPosition);
        AlienQueue firstBirdQueue =
            new(contentManager, spriteBatch, _graphics, AlienEnum.BIRD, originalPosition + marginBottom);
        AlienQueue secondBirdQueue =
            new(contentManager, spriteBatch, _graphics, AlienEnum.BIRD, originalPosition + marginBottom * 2);
        AlienQueue firstFrontQueue =
            new(contentManager, spriteBatch, _graphics, AlienEnum.FRONT, originalPosition + marginBottom * 3);
        AlienQueue secondFrontQueue =
            new(contentManager, spriteBatch, _graphics, AlienEnum.FRONT, originalPosition + marginBottom * 4);

        var allEnemies = shooterQueue.GetEnemies()
            .Concat(firstBirdQueue.GetEnemies())
            .Concat(secondBirdQueue.GetEnemies())
            .Concat(firstFrontQueue.GetEnemies())
            .Concat(secondFrontQueue.GetEnemies());

        foreach (var enemy in allEnemies) _enemies.Add(enemy);
    }

    public IShapeF Bounds { get; }

    public void Draw()
    {
        foreach (var enemy in _enemies) enemy.Draw();
    }

    public void Update()
    {
        IncreaseXAllEnemy();
        InvertDirectionAndFallIfEnemyLimitIsFilled();
    }

    private void IncreaseXAllEnemy()
    {
        foreach (var enemy in _enemies) enemy.IncreaseX(_speed);
    }

    private void InvertDirectionAndFallIfEnemyLimitIsFilled()
    {
        if (!(from enemy in _enemies
                let isRightLimited = enemy.Bounds.Position.X + enemy.GetTexture().Width >=
                                     _graphics.PreferredBackBufferWidth
                let isLeftLimited = enemy.Bounds.Position.X <= 0
                where (isRightLimited || isLeftLimited) && !enemy.IsDead()
                select enemy).Any()) return;
        _enemies.ForEach(e =>
        {
            e.InvertDirection();
            e.Fall();
        });
        _speed += 0.5f;
    }

    public List<IEnemyGroup> GetEnemies()
    {
        return _enemies;
    }
}