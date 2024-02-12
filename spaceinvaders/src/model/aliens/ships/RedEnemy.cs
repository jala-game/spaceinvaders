using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class RedEnemy : IEnemyEntity
{
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool isDead = false;
    private readonly float ALIEN_SPEED_X = 2.5f;
    private readonly Random random = new();
    private readonly int isRightOrLeft;


    public RedEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/red-alien-ship");
        _graphics = graphics;

        isRightOrLeft = random.Next(0, 2);

        int height = 30;
        int widthInitialLocation = isRightOrLeft == 0 ? -50 : _graphics.PreferredBackBufferWidth + 50;
        int width = widthInitialLocation - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
        _texture = texture;
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        isDead = true;
    }

    public void Update()
    {
        IsOutsideFromMap();
        Movement();
    }

    private void IsOutsideFromMap() {
        bool rightLimit = _graphics.PreferredBackBufferWidth > Bounds.Position.X + _texture.Width / 2;
        bool leftLimit = 0 + _texture.Width / 2 < Bounds.Position.X;
        if (isRightOrLeft == 0 && !rightLimit || isRightOrLeft == 1 && !leftLimit)
        {
            isDead = true;
        }
    }

    private void Movement() {
        switch (isRightOrLeft) {
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

    public void Draw()
    {
        _spriteBatch.Draw(
            _texture,
            Bounds.Position,
            Color.White
        );
    }

    public bool IsDead() {
        return isDead;
    }
    
    public int GetPoint()
    {
        Random random = new();
        int[] points = new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        return points[random.Next(0, points.Length)];
    }
}