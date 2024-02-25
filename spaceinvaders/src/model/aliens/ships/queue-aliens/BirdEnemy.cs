using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class BirdEnemy : IEnemyGroup {
    public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool isDead = false;
    private bool directionRight = true;
    private int _point = 20;

    public BirdEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x, int y) {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/bird-alien-ship");
        _graphics = graphics;
        _texture = texture;

        int height = y;
        int width = x - texture.Width / 2;
        Vector2 position = new(width, height);
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


    public void InvertDirection() {
        directionRight = !directionRight;
    }

    public void IncreaseX(float value) {
        float valueModified = directionRight ? value : -value;
        Bounds.Position += new Vector2(valueModified, 0);
    }

    public void Fall() {
        Bounds.Position += new Vector2(0, 50);
    }

    public Bullet GetBullet()
    {
        return null;
    }

    public int GetPoint()
    {
        return _point;
    }


    public Texture2D GetTexture() {
        return _texture;
    }
    
    
}