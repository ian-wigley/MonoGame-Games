using Microsoft.Xna.Framework;

namespace JetPacReloaded;

public class TileNoAnimation(int value, int x, int y, TileCollisionType tileCollisionType)
    : Tile(value, x, y, tileCollisionType)
{
    public override Rectangle TileTexture => new(TextureX, TextureY, MWidth, MHeight);
}