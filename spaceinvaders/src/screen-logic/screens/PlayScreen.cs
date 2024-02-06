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
       this.SpaceShipBulletUpdate();
       this.EntitiesUpdate();

        _collisionComponent.Update(gameTime);
        base.Update(gameTime);
    }

    private void SpaceShipBulletUpdate() {
        spaceShip.bullet?.Update();

        // foreach (Bullet bullet in enemy.bullets) {
        //     bullet.Update();
        //     if (shield.Bound.Intersects(bullet.Bounds)) {
        //         // shield collision
        //     }

        //     if (spaceShip.Bounds.Intersects(bullet.Bounds)) {  You can use that to detect collisions between enemy and the bullet
        //         spaceShip.OnCollision(null); // spaceShip collision
        //     }
        // }
    }

    private void EntitiesUpdate() {
        foreach (Entity entity in entities) {
            entity.Update();
        }
    }

    public override void Draw(GameTime gameTime)
    {

        this.DrawSpaceShip();
        this.DrawEntities();

        base.Draw(gameTime);
    }

    private void DrawSpaceShip() {
        spaceShip.bullet?.Draw();
    }

    private void DrawEntities() {
        foreach (Entity entity in entities) {
            entity.Draw();
        }
    }
}