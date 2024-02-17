using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class FrontEnemy : IEnemyGroup {
        public IShapeF Bounds { get; }

    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool isDead = false;
    private bool directionRight = true;
    private float rotator = 0;
    private int _point = 10;

    public FrontEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x, int y) {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/front-alien-ship");
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
        rotator +=  directionRight ? 0.1f : -0.1f;
    }

    public void Draw()
    {
        Vector2 origin = new(_texture.Width / 2, _texture.Height / 2);

    _spriteBatch.Draw(
        _texture,
        new Vector2(Bounds.Position.X + _texture.Width / 2, Bounds.Position.Y + _texture.Height / 2),
        null,
        Color.White,
        rotator,
        origin,
        Vector2.One,
        SpriteEffects.None,
        0f
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
        Bounds.Position += new Vector2(0, _texture.Height + _texture.Height / 2 - 8);
    }

    public Bullet GetBullet()
    {
        return null;
    }

    public Texture2D GetTexture() {
        return _texture;
    }
    
    public int GetPoint()
    {
        return _point;
    }
    
}