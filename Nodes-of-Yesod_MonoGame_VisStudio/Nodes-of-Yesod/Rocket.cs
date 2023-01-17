using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nodes_of_Yesod
{
    class Rocket
    {
        private int mRocketX = 0;
        private int mRocketScreen = 0;
        private int mRocketFrame = 16;
        private Texture2D mSprites;
        int spriteWidth = 64;
        int spriteHeight = 69;

        public Rocket(Texture2D sprite)
        {
            mSprites = sprite;
        }

        public void Update(GameTime gameTime)
        {
            if (mRocketX >= 0 && mRocketX <= 800)
            {
                mRocketX += 1;
            }
            if (mRocketX >= 801)
            {
                mRocketX = 0;
                mRocketScreen += 1;
            }
            if (mRocketScreen > 15)
            {
                mRocketScreen = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprites, new Vector2(mRocketX, 100.0f), RocketRect, Color.White);
        }

        public Rectangle RocketRect
        {
            get
            {
                return new Rectangle(mRocketFrame * spriteWidth, 7 * spriteHeight, spriteWidth, spriteHeight);
            }
        }

        public int RocketScreen
        {
            get { return mRocketScreen; }
        }
    }
}
