using Microsoft.Xna.Framework;

namespace Nodes_of_Yesod;

public class Tiled
{
    private const int m_width = 62;
    private const int m_height = 48;
    private const int m_tileTextureTotalWidth = 1182;

    public Tiled(int value, int x, int y)
    {
        Value = value;
        X = x;
        Y = y;
        TextureX = value * m_width % m_tileTextureTotalWidth;
        TextureY = 0;
        if (value >= 19 && value < 38)
            TextureY = m_height;
        else if (value >= 38 && value < 57)
            TextureY = m_height * 2;
        else if (value >= 57 && value < 76) TextureY = m_height * 3;
    }

    public int X { get; }
    public int Y { get; }
    public int TextureX { get; }
    public int TextureY { get; }
    public int Value { get; private set; }

    public Rectangle TileRect => new(X, Y, m_width, m_height);

    public Rectangle TileTexture => new(TextureX, TextureY, m_width, m_height);
}