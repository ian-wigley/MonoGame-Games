using Microsoft.Xna.Framework;

namespace Nodes_of_Yesod;

public class Tiled
{
    private const int MWidth = 62;
    private const int MHeight = 48;
    private const int MTileTextureTotalWidth = 1182;

    public Tiled(int value, int x, int y)
    {
        Value = value;
        X = x;
        Y = y;
        TextureX = value * MWidth % MTileTextureTotalWidth;
        TextureY = 0;
        if (value >= 19 && value < 38)
            TextureY = MHeight;
        else if (value >= 38 && value < 57)
            TextureY = MHeight * 2;
        else if (value >= 57 && value < 76) TextureY = MHeight * 3;
    }

    public int X { get; }
    public int Y { get; }
    public int TextureX { get; }
    public int TextureY { get; }
    public int Value { get; private set; }

    public Rectangle TileRect => new(X, Y, MWidth, MHeight);

    public Rectangle TileTexture => new(TextureX, TextureY, MWidth, MHeight);
}