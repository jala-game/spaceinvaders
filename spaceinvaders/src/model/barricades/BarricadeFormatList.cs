using System.Collections.Generic;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model;

public static class BarricadeFormatList
{
    private static readonly Dictionary<BarricadeGeometry, BarricadePositions> Formats = new()
    {
        { BarricadeGeometry.LeftTriangle, new BarricadePositions(0, 0, 26) },
        { BarricadeGeometry.Square, new BarricadePositions(0, 26, 26) },
        { BarricadeGeometry.RightTriangle, new BarricadePositions(0, 53, 26) },
        { BarricadeGeometry.LittleLeftTriangle, new BarricadePositions(0, 79, 10) },
        { BarricadeGeometry.LittleRightTriangle, new BarricadePositions(0, 89, 10) }
    };

    public static BarricadePositions GetFormat(BarricadeGeometry geometry)
    {
        return Formats[geometry];
    }
}