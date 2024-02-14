using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class AlienRound : IEnemyEntity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    public Texture2D Texture { get; set; }
    public Rectangle Rect { get; set; }
    private readonly List<IEnemyEntity> enemies = new();

    public AlienRound(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        _graphics = graphics;
        _spriteBatch = spriteBatch;
        var ORIGINAL_POSITION = 100;
        var MARGIN_BOTTOM = 70;
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

        List<AlienQueue> allLogic = new()
        {
            shooterQueue,
            firstBirdQueue,
            secondBirdQueue,
            firstFrontQueue,
            secondFrontQueue
        };


        foreach (var enemy in allEnemies) enemies.Add(enemy);

        foreach (var queue in allLogic) enemies.Add(queue);
    }

    public IShapeF Bounds { get; }

    public void Draw()
    {
        foreach (var enemy in enemies) enemy.Draw();
    }

    public bool IsDead()
    {
        throw new NotImplementedException();
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        foreach (var enemy in enemies) enemy.Update();
    }

    public List<IEnemyEntity> GetEnemies()
    {
        return enemies;
    }
}