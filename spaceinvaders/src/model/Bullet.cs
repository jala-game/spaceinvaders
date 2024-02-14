using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders;
using spaceinvaders.model;

public class Bullet : DrawableGameComponent, Entity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    public Texture2D Texture { get; set; }
    public Rectangle Rect { get; set; }
    private readonly float SPEED = 25f;

    public Bullet(Game game, Vector2 position, Texture2D texture, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,
        int shipTextureWidth) : base(game)
    {
        Texture = texture;
        float bulletWidth = texture.Width / 2;
        float shipWidth = shipTextureWidth / 2;
        Vector2 bulletPosition = new(position.X + shipWidth - bulletWidth, position.Y + texture.Height / 2 + 50);
        RectangleF rectangleF = new RectangleF(bulletPosition, new Size2(texture.Width, texture.Height));
        Bounds = rectangleF;
        _spriteBatch = spriteBatch;
        _graphics = graphics;
        Game.Services.AddService(typeof(Bullet), this);
    }

    public IShapeF Bounds { get; }

    public void Update()
    {
        Bounds.Position = new Vector2(Bounds.Position.X, Bounds.Position.Y - SPEED);
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            Texture,
            Bounds.Position,
            Color.White
        );
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        if (collisionInfo.Other is BarricadeBlockPart barricadeBlockPart)
        {
            barricadeBlockPart.TakeDamage();
        }
    }
}