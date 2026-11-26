using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

internal class Fuel : BaseObject
{
    private bool _landedOnLedge;

    public Fuel(Texture2D image)
    {
        Image = image;
        Width = image.Width;
        Height = image.Height + 2;
        ScreenLocation = new Vector2(Rand.Next(0, 750), -30);
    }

    public Rectangle FuelRect => Rect;

    public void Update(List<Ledge> ledges)
    {
        Rect = new Rectangle((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);

        if (_landedOnLedge) return;
        foreach (var ledge in ledges)
            if (!Rect.Intersects(ledge.LedgeRect))
                ScreenLocation.Y += 0.2f;
            else
                _landedOnLedge = true;
    }

    public void UpdatePosition(int x, int y)
    {
        ScreenLocation.X = x;
        ScreenLocation.Y = y;
    }

    public bool LowerFuel()
    {
        ScreenLocation.X = 440;
        if (ScreenLocation.Y < 450) ScreenLocation.Y++;
        return ScreenLocation.Y > 448;
    }
}