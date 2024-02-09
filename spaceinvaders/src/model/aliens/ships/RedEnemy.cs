using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class RedEnemy : IEnemyEntity
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private readonly float ALIEN_SPEED_X = 2.5f;
    private readonly int isRightOrLeft;
    private readonly Random random = new();
    private bool isDead;


    public RedEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        var texture = contentManager.Load<Texture2D>("aliens/red-alien-ship");
        _graphics = graphics;

        isRightOrLeft = random.Next(0, 2);

        var height = 30;
        var widthInitialLocation = isRightOrLeft == 0 ? -50 : _graphics.PreferredBackBufferWidth + 50;
        var width = widthInitialLocation - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
        _texture = texture;
    }

    public IShapeF Bounds { get; }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        isDead = true;
    }

    public void Update()
    {
        IsOutsideFromMap();
        Movement();
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            _texture,
            Bounds.Position,
            Color.White
        );
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void IsOutsideFromMap()
    {
        var rightLimit = _graphics.PreferredBackBufferWidth > Bounds.Position.X + _texture.Width / 2;
        var leftLimit = 0 + _texture.Width / 2 < Bounds.Position.X;
        if ((isRightOrLeft == 0 && !rightLimit) || (isRightOrLeft == 1 && !leftLimit)) isDead = true;
    }

    private void Movement()
    {
        switch (isRightOrLeft)
        {
            case 0:
                Vector2 onceRight = new(ALIEN_SPEED_X, 0);
                Bounds.Position += onceRight;
                break;
            case 1:
                Vector2 onceLeft = new(-ALIEN_SPEED_X, 0);
                Bounds.Position += onceLeft;
                break;
        }
    }
}