namespace spaceinvaders.model.barricades;

public class BarricadePositions(int x, int y, int blockSize)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int BlockSize { get; } = blockSize;
}