using Microsoft.Xna.Framework.Graphics;

public interface IEnemyGroup : IEnemyEntity {
    public void IncreaseX(float x);
    public void InvertDirection();
    public Texture2D GetTexture();
    public void Fall();

    public Bullet GetBullet();
    public int GetPoint();

}