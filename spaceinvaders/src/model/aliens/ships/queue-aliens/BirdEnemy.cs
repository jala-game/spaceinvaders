using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using spaceinvaders.model;
using spaceinvaders.model.aliens.ships.queue_aliens;

public class BirdEnemy : IEnemyGroup
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly int _point = 20;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private bool _directionRight = true;
    private bool _isDead;

    public BirdEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x,
        int y)
    {
        var texture = contentManager.Load<Texture2D>("aliens/bird-alien-ship");
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
    }

    public void Draw()
    {
        _spriteBatch.Draw(
            _texture,
            Bounds.Position,
            Color.White
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

    public int GetPoint()
    {
        return _point;
    }


    public Texture2D GetTexture()
    {
        return _texture;
    }
}