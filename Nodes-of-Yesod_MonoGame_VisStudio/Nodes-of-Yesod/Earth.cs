using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

internal class Earth(Texture2D sprite)
{
    private const double ElapsedSecs = 0.1f;
    private double animTimer;
    private const int EarthX = 600;
    private const int EarthY = 20;
    private const int SpriteHeight = 69;
    private const int SpriteWidth = 64;   
    private int earthFrame;

    private Rectangle EarthRect => new(earthFrame * SpriteWidth, 10 * SpriteHeight, SpriteWidth, SpriteHeight);

    public void Update(GameTime gameTime)
    {
        animTimer += ElapsedSecs;
        if (animTimer > 1.2)
        {
            animTimer = 0;
            earthFrame++;
        }

        if (earthFrame >= 15) earthFrame = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(EarthX, EarthY), EarthRect, Color.White);
    }
}