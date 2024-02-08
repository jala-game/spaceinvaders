using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class ShooterEnemy : IEnemyGroup
{
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private Vector2 originalPosition;
    private bool isDead = false;

    public ShooterEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x) {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/shooter-alien-ship");
        _graphics = graphics;
        _texture = texture;

        int height = 200;
        int width = x - texture.Width / 2;
        Vector2 position = new(width, height);
        originalPosition = position;
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
    }

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
            _texture,
            Bounds.Position,
            Color.White
        );
    }

    public void SetInitialPosition() {
        Bounds.Position = originalPosition;
    }

    public void IncreaseX(int value) {
        Bounds.Position += new Vector2(value, 0);
    }

    public Texture2D GetTexture() {
        return _texture;
    }
}