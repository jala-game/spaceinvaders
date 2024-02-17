using System.Collections.Generic;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model;

public static class BarricadeFormatList
{
    private static readonly Dictionary<BarricadeGeometry, BarricadePositions> Formats = new()
    {
        { BarricadeGeometry.LeftTriangle, new BarricadePositions(0, 0, 25) },
        { BarricadeGeometry.Square, new BarricadePositions(0, 25, 25) },
        { BarricadeGeometry.RightTriangle, new BarricadePositions(0, 50, 25) },
        { BarricadeGeometry.LittleLeftTriangle, new BarricadePositions(0, 75, 12) },
        { BarricadeGeometry.LittleRightTriangle, new BarricadePositions(0,88, 12) }
    };

    public static BarricadePositions GetFormat(BarricadeGeometry geometry)
    {
        return Formats[geometry];
    }
}