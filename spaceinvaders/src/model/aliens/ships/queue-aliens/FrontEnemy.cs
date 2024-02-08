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

    public void IncreaseX(int value) {
        int valueModified = directionRight ? value : -value;
        Bounds.Position += new Vector2(valueModified, 0);
    }

    public void Fall() {
        Bounds.Position += new Vector2(0, _texture.Height + 10);
    }

    public Texture2D GetTexture() {
        return _texture;
    }
}