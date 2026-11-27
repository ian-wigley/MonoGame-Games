using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Rocket : BaseObject
{
    public Rocket(int x, int y, Texture2D image, int frame, int width, int height)
    {
        Image = image;
        Frame = frame;
        Width = width;
        Height = height;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
        ScreenLocation = new Vector2(x, y);
    }

    public Rectangle RocketRect => new((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);

    public void Update(int x, int y)
    {
        ScreenLocation.X = x;
        ScreenLocation.Y = y;
    }

    public bool LowerSectionOne()
    {
        ScreenLocation.X = 422;
        if (ScreenLocation.Y < 383) ScreenLocation.Y++;
        return ScreenLocation.Y >= 383;
    }

    public bool LowerSectionTwo()
    {
        ScreenLocation.X = 422;
        if (ScreenLocation.Y < 323) ScreenLocation.Y++;
        return ScreenLocation.Y >= 323;
    }

    public bool TakeOff()
    {
        ScreenLocation.Y -= 1.5f;
        return ScreenLocation.Y > -200;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Image, ScreenLocation, Rect, Color.White, 0, Vector2.Zero, 1.0f, 0, 0);
    }
}