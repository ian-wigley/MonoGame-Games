using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Ledge : BaseObject
{
    public Ledge(int x, int y, Texture2D image)
    {
        Image = image;
        Height = image.Height;
        Width = image.Width;
        Rect = new Rectangle(x, y, Width, Height);
        ScreenLocation = new Vector2(x, y);
    }

    public Rectangle LedgeRect => Rect;

    public void Update(int x, int y)
    {
        ScreenLocation.X += x;
        ScreenLocation.Y += y;
        Rect = new Rectangle((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);
    }
}