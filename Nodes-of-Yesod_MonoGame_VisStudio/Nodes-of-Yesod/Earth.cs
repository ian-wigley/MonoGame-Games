using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

internal class Earth(Texture2D sprite)
{
    private const double elapsedSecs = 0.1f;

    private double animTimer;
    private int mEarthFrame;
    private readonly int mEarthX = 600;
    private readonly int mEarthY = 20;
    private readonly int mSpriteHeight = 69;
    private readonly int mSpriteWidth = 64;

    public Rectangle EarthRect => new(mEarthFrame * mSpriteWidth, 10 * mSpriteHeight, mSpriteWidth, mSpriteHeight);

    public void Update(GameTime gameTime)
    {
        animTimer += elapsedSecs;
        if (animTimer > 1.2)
        {
            animTimer = 0;
            mEarthFrame++;
        }

        if (mEarthFrame >= 15) mEarthFrame = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(mEarthX, mEarthY), EarthRect, Color.White);
    }
}