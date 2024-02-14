using System.Collections.Generic;

namespace spaceinvaders.model;

public static class BarricadeFormatList
{
    private static readonly Dictionary<BarricadeGeometry, BarricadePositions> Formats = new()
    {
        { BarricadeGeometry.LEFT_TRIANGLE, new BarricadePositions(0, 0, 26) },
        { BarricadeGeometry.SQUARE, new BarricadePositions(0, 26, 26) },
        { BarricadeGeometry.RIGHT_TRIANGLE, new BarricadePositions(0, 53, 26) },
        { BarricadeGeometry.LITTLE_LEFT_TRIANGLE, new BarricadePositions(0, 79, 10) },
        { BarricadeGeometry.LITTLE_RIGHT_TRIANGLE, new BarricadePositions(0, 89, 10) }
    };

    public static BarricadePositions GetFormat(BarricadeGeometry geometry)
    {
        return Formats[geometry];
    }
}
