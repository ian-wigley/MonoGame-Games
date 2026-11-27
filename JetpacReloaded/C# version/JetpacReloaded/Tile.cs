using Microsoft.Xna.Framework;

namespace JetPacReloaded;

/// <summary>
///     Controls the collision detection and response behavior of a tile.
/// </summary>
public enum TileCollisionType
{
    /// <summary>
    ///     A passable tile is one which does not hinder player motion at all.
    /// </summary>
    Passable = 0,

    /// <summary>
    ///     An impassable tile is one which does not allow the player to move through
    ///     it at all. It is completely solid.
    /// </summary>
    Impassable = 1,

    /// <summary>
    ///     A platform tile is one which behaves like a passable tile except when the
    ///     player is above it. A player can jump up through a platform as well as move
    ///     past it to the left and right, but can not fall down through the top of it.
    /// </summary>
    Platform = 2,

    Wall = 4
}

public class Tile(int value, int x, int y, TileCollisionType tileCollisionType)
{
    protected const int MWidth = 62;
    protected const int MHeight = 48;

    protected int X { get; } = x;
    protected int Y { get; } = y;
    protected int TextureX { get; set; } = value * MWidth;
    protected int TextureY { get; private set; } = 0; //  value * m_height;
    protected int Value { get; private set; } = value;

    public TileCollisionType GetTileCollisionType { get; private set; } = tileCollisionType;

    public Rectangle TileRect => new(X, Y, MWidth, MHeight);

    public virtual Rectangle TileTexture { get; }

    public bool Drawable(int x, int y)
    {
        //var draw = X < x + 800 && Y < y + 600;
        //return X < x + 800 && Y < y + 600;
        return true;
    }

    //protected Rectangle TileTexture { get { return new Rectangle(TextureX, TextureY, m_width, m_height); } }
}