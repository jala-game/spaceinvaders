using Microsoft.Xna.Framework.Graphics;

public interface IEnemyGroup : IEnemyEntity
{
    public void IncreaseX(int x);
    public void InvertDirection();
    public Texture2D GetTexture();
    public void Fall();
}