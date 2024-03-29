using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienRound
{
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly List<IEnemyGroup> enemies = [];
    private readonly List<IEnemyEntity> logics = [];
    private float SPEED = 1f;


    public AlienRound(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
        _graphics = graphics;
        _spriteBatch = spriteBatch;
        int ORIGINAL_POSITION = 100;
        int MARGIN_BOTTOM = 70;
        AlienQueue shooterQueue =
            new(contentManager, _spriteBatch, _graphics, AlienEnum.SHOOTER, ORIGINAL_POSITION);
        AlienQueue firstBirdQueue =
            new(contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, ORIGINAL_POSITION + MARGIN_BOTTOM);
        AlienQueue secondBirdQueue =
            new(contentManager, _spriteBatch, _graphics, AlienEnum.BIRD, ORIGINAL_POSITION + MARGIN_BOTTOM * 2);
        AlienQueue firstFrontQueue =
            new(contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, ORIGINAL_POSITION + MARGIN_BOTTOM * 3);
        AlienQueue secondFrontQueue =
            new(contentManager, _spriteBatch, _graphics, AlienEnum.FRONT, ORIGINAL_POSITION + MARGIN_BOTTOM * 4);

        var allEnemies = shooterQueue.GetEnemies()
        .Concat(firstBirdQueue.GetEnemies())
        .Concat(secondBirdQueue.GetEnemies())
        .Concat(firstFrontQueue.GetEnemies())
        .Concat(secondFrontQueue.GetEnemies());

        foreach (var enemy in allEnemies) {
            enemies.Add(enemy);
        }
    }

    public void Draw()
    {
        foreach (IEnemyEntity enemy in enemies) {
            enemy.Draw();
        }
    }

    public void Update()
    {
        IncreaseXAllEnemy();
        InvertDirectionAndFallIfEnemyLimitIsFilled();
    }

    private void IncreaseXAllEnemy() {
        foreach (IEnemyGroup enemy in enemies) {
            enemy.IncreaseX(SPEED);
        }
    }

    private void InvertDirectionAndFallIfEnemyLimitIsFilled() {
        foreach (IEnemyGroup enemy in enemies) {
            bool isRightLimited = enemy.Bounds.Position.X + enemy.GetTexture().Width >= _graphics.PreferredBackBufferWidth;
            bool isLeftLimited = enemy.Bounds.Position.X <= 0;
            if ((isRightLimited || isLeftLimited) && !enemy.IsDead()) {
                enemies.ForEach(e => {
                    e.InvertDirection();
                    e.Fall();
                });
                SPEED+= 0.5f;
                return;
            }
        }
    }

    public List<IEnemyGroup> GetEnemies() {
        return enemies;
    }

    public List<IEnemyEntity> GetLogics() {
        return logics;
    }
}