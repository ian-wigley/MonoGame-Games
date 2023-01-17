using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nodes_of_Yesod
{
    class WhirlWind : Enemy
    {
        private int mWhirlWindX = 0;
        private int mWhirlWindY = 0;
        private int mWhirlWindFrame = 17;

        public WhirlWind(float xPos, float yPos, float speedX, float speedY, Texture2D sprite, List<Rectangle> walls)
            : base(xPos, yPos, speedX, speedY, sprite, walls)
        {
            mWhirlWindX = 2;
            mWhirlWindY = 9;
            SheetSize = 3;
            timeSinceLastFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > 0.6)
            {
                timeSinceLastFrame = 0;
                mWhirlWindFrame++;
            }
            if (mWhirlWindFrame >= 6)
            {
                mWhirlWindFrame = 0;
            }
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, new Vector2(mWhirlWindX, mWhirlWindY), WhirlWindRect, Color.White);
        }

        public Rectangle WhirlWindRect
        {
            get
            {
                return new Rectangle(mWhirlWindFrame * mSpriteWidth, 9 * mSpriteHeight, mSpriteWidth, mSpriteHeight);
            }
        }
    }
}
