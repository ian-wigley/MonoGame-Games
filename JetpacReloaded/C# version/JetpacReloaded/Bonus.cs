using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

internal class Bonus : BaseObject
{
    private bool _bonusLanded;
    private int _prevFrame;

    public Bonus(Texture2D image)
    {
        Frame = Rand.Next(0, 4);
        Image = image;
        Width = image.Width / 5;
        Height = image.Height + 2;
        ScreenLocation = new Vector2(Rand.Next(0, 750), -30);
    }

    public Rectangle BonRect => new((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);

    public bool BonusLanded
    {
        set => _bonusLanded = value;
    }

    public void Update()
    {
        if (!_bonusLanded) ScreenLocation.Y++;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        var bonusLocation = new Vector2(ScreenLocation.X, ScreenLocation.Y);
        spriteBatch.Draw(Image, bonusLocation, Rect, Color.White, 0, Vector2.Zero, 1.0f, 0, 0);
    }

    public void Reset()
    {
        _prevFrame = Frame;
        Frame = Rand.Next(0, 4);
        if (Frame == _prevFrame) Frame = (Frame + 1) % 4;
        ScreenLocation.X = Rand.Next(0, 750);
        ScreenLocation.Y = -30;
        _bonusLanded = false;
    }
}