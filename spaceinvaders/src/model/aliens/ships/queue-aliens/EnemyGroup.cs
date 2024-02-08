using Microsoft.Xna.Framework.Graphics;

public interface IEnemyGroup : IEnemyEntity {
    public void IncreaseX(int x);
    public void SetInitialPosition();
    public Texture2D GetTexture();
}