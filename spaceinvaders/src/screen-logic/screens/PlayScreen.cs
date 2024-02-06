using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class PlayScreen : GameScreenModel
{

    private readonly SpaceShip spaceShip;
    private CollisionComponent _collisionComponent;
    private readonly GraphicsDeviceManager _graphics;
    private readonly List<Entity> entities = new();

    public PlayScreen(SpaceShip ship, GraphicsDeviceManager graphics)
    {
        spaceShip = ship;
        _graphics = graphics;
    }

    public override void Initialize() {
        base.Initialize();
        _collisionComponent = new CollisionComponent(new RectangleF(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

        entities.Add(spaceShip);


        foreach (Entity entity in entities) {
            _collisionComponent.Insert(entity);
        }
    }

    public override void LoadContent() {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (Bullet bullet in spaceShip.bullets) {
            bullet.Update();
            if (spaceShip.Bounds.Intersects(bullet.Bounds)) {
                spaceShip.OnCollision(null);
            }
        }

        foreach (Entity entity in entities) {
            entity.Update();
        }

        _collisionComponent.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (Bullet bullet in spaceShip.bullets) {
            bullet.Draw();
        }

        foreach (Entity entity in entities) {
            entity.Draw();
        }

        base.Draw(gameTime);
    }
}