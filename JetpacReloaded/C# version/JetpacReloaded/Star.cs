using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetPacReloaded;

public class Star : BaseObject
{
    private readonly int _starLayer1Y = Rand.Next(50, 440);
    private readonly int _starLayer2Y = Rand.Next(52, 440);
    private int _starLayer1X = Rand.Next(0, 800);

    private int _starLayer2X = Rand.Next(0, 800);

    public Star(Texture2D image)
    {
        Image = image;
    }

    public void Update()
    {
        if (_starLayer1X < 800)
            _starLayer1X++;
        else
            _starLayer1X = Rand.Next(-400, 0);

        if (_starLayer2X < 800)
            _starLayer2X += 2;
        else
            _starLayer2X = Rand.Next(-400, 0);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        var starLocationLayer1 = new Vector2(_starLayer1X, _starLayer1Y);
        spriteBatch.Draw(Image, starLocationLayer1, Color.DarkGray);

        var starLocationLayer2 = new Vector2(_starLayer2X, _starLayer2Y);
        spriteBatch.Draw(Image, starLocationLayer2, Color.White);
    }
}