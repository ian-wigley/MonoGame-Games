using Microsoft.Xna.Framework;

namespace Nodes_of_Yesod
{
    public class Tile
    {
        const int m_width = 62;
        const int m_height = 48;
        const int m_tileTextureTotalWidth = 1182;

        public Tile(int value, int x, int y)
        {
            Value = value;
            X = x;
            Y = y;
            TextureX = (value * m_width) % m_tileTextureTotalWidth;
            TextureY = 0;
            if (value >= 19 && value < 38)
            {
                TextureY = m_height;
            }
            else if (value >= 38 && value < 57)
            {
                TextureY = m_height * 2;
            }
            else if (value >= 57 && value < 76)
            {
                TextureY = m_height * 3;
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int TextureX { get; private set; }
        public int TextureY { get; private set; }
        public int Value { get; private set; }

        public Rectangle TileRect { get { return new Rectangle(X, Y, m_width, m_height); } }

        public Rectangle TileTexture { get { return new Rectangle(TextureX, TextureY, m_width, m_height); } }
    }
}
