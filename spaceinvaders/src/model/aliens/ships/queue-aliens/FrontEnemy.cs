using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;
using spaceinvaders.model.aliens.ships.queue_aliens;

public class FrontEnemy : IEnemyGroup
{
    private const int Point = 10;
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool _directionRight = true;
    private bool _isDead;
    private float _rotator;

    public FrontEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x,
        int y)
    {
        var texture = contentManager.Load<Texture2D>("aliens/front-alien-ship");
        _graphics = graphics;
        _texture = texture;

        var height = y;
        var width = x - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
    }

    public IShapeF Bounds { get; }

    public bool IsDead()
    {
        return _isDead;
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        _isDead = true;
    }

    public void Update()
    {
        _rotator += _directionRight ? 0.1f : -0.1f;
    }

    public void Draw()
    {
        Vector2 origin = new(_texture.Width / 2, _texture.Height / 2);

        _spriteBatch.Draw(
            _texture,
            new Vector2(Bounds.Position.X + _texture.Width / 2, Bounds.Position.Y + _texture.Height / 2),
            null,
            Color.White,
            _rotator,
            origin,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public void InvertDirection()
    {
        _directionRight = !_directionRight;
    }

    public void IncreaseX(float value)
    {
        var valueModified = _directionRight ? value : -value;
        Bounds.Position += new Vector2(valueModified, 0);
    }

    public void Fall()
    {
        Bounds.Position += new Vector2(0, 50);
    }

    public Bullet GetBullet()
    {
        return null;
    }

    public Texture2D GetTexture()
    {
        return _texture;
    }

    public int GetPoint()
    {
        return Point;
    }
}