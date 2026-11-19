using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod;

internal class Rocket(Texture2D sprite)
{
    private const int MRocketFrame = 16;
    private int mRocketX;
    private const int SpriteHeight = 69;
    private const int SpriteWidth = 64;

    public Rectangle RocketRect => new(MRocketFrame * SpriteWidth, 7 * SpriteHeight, SpriteWidth, SpriteHeight);

    public int RocketScreen { get; private set; }

    public void Update(GameTime gameTime)
    {
        if (mRocketX is >= 0 and <= 800) mRocketX += 1;
        if (mRocketX >= 801)
        {
            mRocketX = 0;
            RocketScreen += 1;
        }

        if (RocketScreen > 15) RocketScreen = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Vector2(mRocketX, 100.0f), RocketRect, Color.White);
    }
}