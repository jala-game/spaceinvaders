using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class FrontEnemy : IEnemyGroup
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    public Texture2D Texture { get; set; }
    public Rectangle Rect { get; set; }
    private bool directionRight = true;
    private bool isDead;
    private float rotator;

    public FrontEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x,
        int y)
    {
        var texture = contentManager.Load<Texture2D>("aliens/front-alien-ship");
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
        rotator += directionRight ? 0.1f : -0.1f;
    }

    public void Draw()
    {
        Vector2 origin = new(Texture.Width / 2, Texture.Height / 2);

        _spriteBatch.Draw(
            Texture,
            new Vector2(Bounds.Position.X + Texture.Width / 2, Bounds.Position.Y + Texture.Height / 2),
            null,
            Color.White,
            rotator,
            origin,
            Vector2.One,
            SpriteEffects.None,
            0f
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
        Bounds.Position += new Vector2(0, Texture.Height + Texture.Height / 2 - 8);
    }

    public Texture2D GetTexture()
    {
        return Texture;
    }
}