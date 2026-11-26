using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Enemy : BaseObject
{
    public Enemy(Texture2D image)
    {
        Image = image;
        Width = image.Width / 8;
        Height = image.Height;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
        ScreenLocation = new Vector2(Rand.Next(0, 800), Rand.Next(50, 440));
    }

    public Rectangle AlienRect => new((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);

    public void Update(int level)
    {
        if (ScreenLocation.X > -70)
            ScreenLocation.X--;
        else
            ResetMeteor();
        Frame = level % 8;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
    }

    public void ResetMeteor()
    {
        ScreenLocation.X = Rand.Next(800, 1200);
        ScreenLocation.Y = Rand.Next(50, 400);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Image, ScreenLocation, Rect, Color.White, 0, Vector2.Zero, 1.0f, 0, 0);
    }
}