using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class JetMan : BaseObject
{
    private readonly Texture2D _debugImage;
    private SpriteEffects _flip = SpriteEffects.None;

    public JetMan(int x, int y, Texture2D image, Texture2D debugImage)
    {
        NumberOfFrames = 8;
        Image = image;
        Height = image.Height; // 68;
        Width = image.Width / NumberOfFrames; // 64;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
        ScreenLocation = new Vector2(x, y);
        _debugImage = debugImage;
    }

    public Rectangle JetManRect => new((int)ScreenLocation.X, (int)ScreenLocation.Y, Width, Height);

    public Vector2 JetManPosition => ScreenLocation;

    public int JetManAnimFrame
    {
        set => Frame = value;
    }

    public void Update(Vector2 vec, SpriteEffects flip)
    {
        //                   this.m_frame = this.m_frame % 4 + 1;
        Frame = Frame % NumberOfFrames + 1;
        ScreenLocation = vec;
        Rect = new Rectangle(Frame * Width, 0, Width, Height);
        _flip = flip;
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(_debugImage, ScreenLocation, JetManRect, Color.White, 0, Vector2.Zero, 1.0f, _flip, 0);
        spriteBatch.Draw(Image, ScreenLocation, Rect, Color.White, 0, Vector2.Zero, 1.0f, _flip, 0);
    }
}