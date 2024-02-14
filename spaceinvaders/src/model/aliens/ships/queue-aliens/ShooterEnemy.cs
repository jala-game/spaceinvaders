using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class ShooterEnemy : IEnemyGroup
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    public Texture2D Texture { get; set; }
    public Rectangle Rect { get; set; }
    private bool directionRight = true;
    private bool isDead;

    public ShooterEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x,
        int y)
    {
        var texture = contentManager.Load<Texture2D>("aliens/shooter-alien-ship");
        _graphics = graphics;
        Texture = texture;

        var height = y;
        var width = x - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
    }

    public IShapeF Bounds { get; }

    public bool IsDead()
    {
        return isDead;
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        isDead = true;
    }

    public void Update()
    {
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            Texture,
            Bounds.Position,
            Color.White
        );
    }

    public void InvertDirection()
    {
        directionRight = !directionRight;
    }

    public void IncreaseX(int value)
    {
        var valueModified = directionRight ? value : -value;
        Bounds.Position += new Vector2(valueModified, 0);
    }

    public void Fall()
    {
        Bounds.Position += new Vector2(0, Texture.Height + 10);
    }

    public Texture2D GetTexture()
    {
        return Texture;
    }
}