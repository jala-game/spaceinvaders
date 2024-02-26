using Microsoft.Xna.Framework.Graphics;

namespace spaceinvaders.model.aliens.ships.queue_aliens;

public interface IEnemyGroup : IEnemyEntity
{
    public void IncreaseX(float x);
    public void InvertDirection();
    public Texture2D GetTexture();
    public void Fall();

    public Bullet GetBullet();
    public int GetPoint();
}