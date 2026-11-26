using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Bullet : BaseObject
{
    private readonly bool _left;

    public Bullet(int x, int y, Texture2D image, bool left)
    {
        Image = image;
        _left = left;
        ScreenLocation = new Vector2(x, y);
    }

    public bool Offscreen { get; private set; }

    public Rectangle BulletRect => new((int)ScreenLocation.X, (int)ScreenLocation.Y, Image.Width, Image.Height);

    public void Update()
    {
        switch (ScreenLocation.X)
        {
            case < 800 when !_left:
                ScreenLocation.X += 6;
                break;
            case > -40:
                ScreenLocation.X -= 6;
                break;
        }

        if (ScreenLocation.X is >= 800 or <= -40) Offscreen = true;
    }
}