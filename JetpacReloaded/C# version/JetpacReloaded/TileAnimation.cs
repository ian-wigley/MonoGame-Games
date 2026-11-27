using Microsoft.Xna.Framework;

namespace JetPacReloaded;

public class TileAnimation(int value, int x, int y, int animFrames, TileCollisionType tileCollisionType)
    : Tile(value, x, y, tileCollisionType)
{
    private int _mFrame; // value * m_width;
    private int _mTimer;

    public override Rectangle TileTexture
    {
        get
        {
            _mTimer += 1;
            if (_mTimer != 20) return new Rectangle(TextureX, TextureY, MWidth, MHeight);
            _mFrame = (_mFrame + 1) % animFrames;
            TextureX = (Value + _mFrame) * MWidth;
            _mTimer = 0;
            return new Rectangle(TextureX, TextureY, MWidth, MHeight);
        }
    }
}