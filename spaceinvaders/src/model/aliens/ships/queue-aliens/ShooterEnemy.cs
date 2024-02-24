using System;
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
    private readonly ContentManager _contentManager;
    private bool isDead = false;
    private bool directionRight = true;
    public Bullet bullet;
    private int _point = 40;

    public ShooterEnemy(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int x, int y) {
        Texture2D texture = contentManager.Load<Texture2D>("aliens/shooter-alien-ship");
        _graphics = graphics;
        _texture = texture;

        int height = y;
        int width = x - texture.Width / 2;
        Vector2 position = new(width, height);
        Bounds = new RectangleF(position, new Size2(texture.Width, texture.Height));

        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
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
        int randomShotValue = RandomShotValue();
        Shoot(randomShotValue);
        RemoveBulletIfIsDead();
        RemoveBulletWhenLeaveFromMap();

    }

    private void Shoot(int randomShotValue){
        if (randomShotValue == 5 && bullet == null){
            Texture2D bulletTexture = _contentManager.Load<Texture2D>("red-bullet");
            bullet = new Bullet(Bounds.Position, bulletTexture, _spriteBatch, _graphics, _texture.Width, TypeBulletEnum.ALIEN);
        }
    }

    private void RemoveBulletWhenLeaveFromMap() {
        if (bullet!= null && bullet.Bounds.Position.Y > _graphics.PreferredBackBufferHeight) {
            bullet = null;
        }
    }

    public void RemoveBulletIfIsDead()
    {
        if (bullet != null && bullet.GetIsDead()) bullet = null;
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
        Bounds.Position += new Vector2(0, _texture.Height + 10);
    }

    public Bullet GetBullet()
    {
        return bullet;
    }

    public Texture2D GetTexture() {
        return _texture;
    }

    private int RandomShotValue(){
        Random random = new Random();
        return random.Next(0, 500);
    }

    public int GetPoint()
    {
        return _point;
    }
}