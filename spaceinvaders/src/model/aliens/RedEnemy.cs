using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class RedEnemy : Entity
{
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    public bool isDead = false;

    private readonly float ALIEN_SPEED_Y = 0f;
    private readonly float ALIEN_SPEED_X = 0f;
    private readonly int changePositionDirectionDelay = 200;
    private int changePositionDirectionCounter = 0;
    private int isRightOrLeft;
    private readonly Random random = new();


    public RedEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/red-alien-ship");
        _graphics = graphics;

        int heightTop = 0;
        int randomWidthPosition = random.Next(-400, 400);
        int widthCenter = _graphics.PreferredBackBufferWidth / 2 - texture.Width / 2;
        Vector2 position = new(widthCenter, heightTop);
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
        bool isInitialMovement = InitialMovement();
        if (isInitialMovement) return;
        // Console.WriteLine(isDead);
        IsOutsideFromMap();
        MovementDelay();
        Movement();
    }

    private bool InitialMovement() {
        if (Bounds.Position.Y < _graphics.PreferredBackBufferHeight / 2) {
            Bounds.Position += new Vector2(0, ALIEN_SPEED_Y);
            return true;
        }

        return false;
    }

    private void IsOutsideFromMap() {
        bool rightLimit = _graphics.PreferredBackBufferWidth > Bounds.Position.X + _texture.Width / 2;
        bool leftLimit = _texture.Width / 2 < Bounds.Position.X;
        bool bottomLimit = Bounds.Position.Y  > _graphics.GraphicsDevice.Viewport.Height * 1.5;

        if (!rightLimit || !leftLimit || bottomLimit) isDead = true;
    }

    private void MovementDelay() {
        if (changePositionDirectionCounter >= changePositionDirectionDelay) {
            isRightOrLeft = random.Next(0, 4);
            changePositionDirectionCounter = 0;
            return;
        }

        changePositionDirectionCounter++;
    }

    private void Movement() {
        switch (isRightOrLeft) {
            case 0:
                Vector2 right = new(ALIEN_SPEED_X, ALIEN_SPEED_Y);
                Bounds.Position += right;
                break;
            case 1:
                Vector2 left = new(-ALIEN_SPEED_X, ALIEN_SPEED_Y);
                Bounds.Position += left;
                break;
            case 2:
                Vector2 onceLeft = new(-ALIEN_SPEED_X, 0);
                Bounds.Position += onceLeft;
                break;
            case 3:
                Vector2 onceRight = new(ALIEN_SPEED_X, 0);
                Bounds.Position += onceRight;
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

}