using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class BaseObject
{
    protected static readonly Random Rand = new();
    protected int Frame;
    protected int Height;
    protected Texture2D Image;
    protected Rectangle Rect;
    protected Vector2 ScreenLocation;
    protected int Width;

    protected BaseObject()
    {
        Width = 0;
        Height = 0;
        Frame = 0;
        Rect = new Rectangle(0, 0, Width, Height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Image, ScreenLocation, Color.White);
    }
}