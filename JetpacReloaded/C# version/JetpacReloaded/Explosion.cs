using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Explosion : BaseObject
{
    public Explosion(int x, int y, Texture2D image)
    {
        Image = image;
        Height = image.Height;
        Width = image.Width / 16;
        ScreenLocation = new Vector2(x, y);
    }

    public bool AnimationComplete { get; private set; }

    public void Update()
    {
        if (Frame < 16)
            Frame += 1;
        else
            AnimationComplete = true;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Image, ScreenLocation, Rect, Color.White, 0, Vector2.Zero, 1.0f, 0, 0);
    }
}